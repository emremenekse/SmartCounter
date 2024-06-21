using AutoMapper;
using Grpc.Net.Client;
using MassTransit;
using ReportService.Abstractions;
using ReportService.DTOs;
using ReportService.Entities;
using ReportService.Messaging;
using CounterService.Grpc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace ReportService.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _counterEndpoint;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, IPublishEndpoint publishEndpoint, IConfiguration configuration, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _configuration = configuration;
            _httpClient = httpClient;
            _baseUrl = configuration["CounterApi:BaseUrl"];
            _counterEndpoint = configuration["CounterApi:CounterEndpoint"];
        }
        public async Task<Shared.Response<ReportRequestDTO>> CreateReportRequestAsync(string serialNumber, DateTime measurementTime)
        {
            var existCheck = await _unitOfWork.ReportRepository.GetRequestBySerialNumberAsyncWithDate(serialNumber,measurementTime);
            if(existCheck != null )
            {
                return Shared.Response<ReportRequestDTO>.Fail("Report request already exists", 400);
            }
            var request = new ReportRequest
            {
                Id = Guid.NewGuid(),
                SerialNumber = serialNumber,
                MeasurementTime = measurementTime,
                RequestTime = DateTime.UtcNow,
                Status = "Pending"
            };

            await _unitOfWork.ReportRepository.AddRequestAsync(request);
            await _unitOfWork.SaveChangesAsync();
            CounterResponse grpcResponse = null;
            try
            {
                var grpcCounterServiceUrl = _configuration["GrpcSettings:CounterServiceUrl"];
                var grpcChannel = GrpcChannel.ForAddress(grpcCounterServiceUrl);
                var grpcClient = new Counter.CounterClient(grpcChannel);
                var grpcRequest = new CounterRequest { SerialNumber = serialNumber, MeasurementTime = measurementTime.ToString() };
                grpcResponse = await grpcClient.GetCounterDataAsync(grpcRequest);
            }
            catch (Exception)
            {
                var fullUrl = $"{_baseUrl}{_counterEndpoint}/{serialNumber}/{measurementTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}";
                var httpCounterServiceUrl = _configuration["HttpSettings:CounterServiceUrl"];
                var httpResponse = await _httpClient.GetAsync(fullUrl);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var meterReadingResponse = JsonSerializer.Deserialize<MeterReadingResponseDTO>(responseContent, options);
                    if (meterReadingResponse != null)
                    {
                        grpcResponse = MapToCounterResponse(meterReadingResponse.Data);
                    }
                    else
                    {
                        throw new Exception("HTTP request failed with message: " + meterReadingResponse?.ErrorMessage + "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" + meterReadingResponse + "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" + responseContent);
                    }
                }
                else
                {
                    throw new Exception(fullUrl + "HTTP request failed with status code: " + "###########" + httpResponse + "###########"+$"{httpCounterServiceUrl}/{serialNumber}/{measurementTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");
                }
            }
            if (grpcResponse == null)
            {
                throw new Exception("Failed to retrieve counter data from both gRPC and HTTP endpoints");
            }

            var reportMessage = new ReportMessage
            {
                ReportRequestId = request.Id,
                SerialNumber = grpcResponse.SerialNumber,
                LastIndex = grpcResponse.LastIndex,
                Voltage = grpcResponse.Voltage,
                Current = grpcResponse.Current,
                MeasurementTime = grpcResponse.MeasurementTime
            };

            await _publishEndpoint.Publish(reportMessage);

            return Shared.Response<ReportRequestDTO>.Success(_mapper.Map<ReportRequestDTO>(request),201);
        }
        private CounterResponse MapToCounterResponse(MeterReadingDTO meterReading)
        {
            return new CounterResponse
            {
                SerialNumber = meterReading.SerialNumber,
                LastIndex = (double)meterReading.LastIndex,
                Voltage = (double)meterReading.Voltage,
                Current = (double)meterReading.Current,
                MeasurementTime = meterReading.MeasurementTime.ToString("o")
            };
        }

        public async Task<Shared.Response<IEnumerable<ReportRequestDTO>>> GetAllReportRequestsAsync()
        {
            var requests = await _unitOfWork.ReportRepository.GetAllRequestsAsync();
            return Shared.Response<IEnumerable<ReportRequestDTO>>.Success(_mapper.Map<IEnumerable<ReportRequestDTO>>(requests),200);
        }

        public async Task<Shared.Response<ReportRequestDTO>> GetReportRequestByIdAsync(Guid requestId)
        {
            var request = await _unitOfWork.ReportRepository.GetRequestByIdAsync(requestId);
            if (request == null)
            {
                return Shared.Response<ReportRequestDTO>.Fail("Report request not found", 404);
            }
            var requestDto = _mapper.Map<ReportRequestDTO>(request);

            if (request.Status == "Completed")
            {
                var result = await _unitOfWork.ReportRepository.GetResultByRequestIdAsync(requestId);
                if (result != null)
                {
                    requestDto.FilePath = result.FilePath;
                }
            }

            return Shared.Response<ReportRequestDTO>.Success(requestDto,200);
        }
    }
}
