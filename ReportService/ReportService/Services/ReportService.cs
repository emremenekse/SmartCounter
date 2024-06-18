using AutoMapper;
using Grpc.Net.Client;
using MassTransit;
using ReportService.Abstractions;
using ReportService.DTOs;
using ReportService.Entities;
using ReportService.Messaging;
using CounterService.Grpc;
using System.Collections.Generic;

namespace ReportService.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Shared.Response<ReportRequestDTO>> CreateReportRequestAsync(string serialNumber)
        {
            var existCheck = await _unitOfWork.ReportRepository.GetRequestBySerialNumberAsync(serialNumber);
            if(existCheck != null )
            {
                return Shared.Response<ReportRequestDTO>.Fail("Report request already exists", 400);
            }
            var request = new ReportRequest
            {
                Id = Guid.NewGuid(),
                SerialNumber = serialNumber,
                RequestTime = DateTime.UtcNow,
                Status = "Pending"
            };

            await _unitOfWork.ReportRepository.AddRequestAsync(request);
            await _unitOfWork.SaveChangesAsync();

            var grpcChannel = GrpcChannel.ForAddress("https://localhost:7088");
            var grpcClient = new Counter.CounterClient(grpcChannel);
            var grpcRequest = new CounterRequest { SerialNumber = serialNumber };
            var grpcResponse = await grpcClient.GetCounterDataAsync(grpcRequest);

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

        public async Task<Shared.Response<IEnumerable<ReportRequestDTO>>> GetAllReportRequestsAsync()
        {
            var requests = await _unitOfWork.ReportRepository.GetAllRequestsAsync();
            return Shared.Response<IEnumerable<ReportRequestDTO>>.Success(_mapper.Map<IEnumerable<ReportRequestDTO>>(requests),200);
        }

        public async Task<Shared.Response<ReportRequestDTO>> GetReportRequestByIdAsync(Guid requestId)
        {
            var request = await _unitOfWork.ReportRepository.GetRequestByIdAsync(requestId);
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
