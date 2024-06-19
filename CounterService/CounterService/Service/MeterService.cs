using AutoMapper;
using CounterService.Abstraction;
using CounterService.DTOs;
using CounterService.Entity;
using Shared;
using System.Collections.Generic;

namespace CounterService.Service
{
    public class MeterService : IMeterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MeterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Shared.Response<IEnumerable<MeterReadingDTO>>> GetAllAsync()
        {
            var meterReadings = await _unitOfWork.MeterRepository.GetAllAsync();
            return Shared.Response<IEnumerable<MeterReadingDTO>>.Success(_mapper.Map<IEnumerable<MeterReadingDTO>>(meterReadings),200);
        }

        public async Task<Shared.Response<MeterReadingDTO>> GetBySerialNumberAndMeasurementTimeAsync(string serialNumber, DateTime measurementTime)
        {
            if (serialNumber == null || serialNumber.Length != 8)
            {
                return Response<MeterReadingDTO>.Fail("Serial number must be 8 characters long.", 400);
            }
            var meterReading = await _unitOfWork.MeterRepository.GetBySerialNumberAndMeasurementTimeAsync(serialNumber, measurementTime);
            if (meterReading == null)
            {
                return Response<MeterReadingDTO>.Fail("Serial number not found.", 404);
            }
            return Shared.Response<MeterReadingDTO>.Success(_mapper.Map<MeterReadingDTO>(meterReading), 200);
        }


        public async Task<Shared.Response<MeterReadingDTO>> AddAsync(MeterReadingDTO meterReadingDto)
        {
            meterReadingDto.Id = Guid.NewGuid();

            var meterReading = _mapper.Map<MeterReading>(meterReadingDto);
            meterReading.MeasurementTime = meterReading.MeasurementTime.ToString();
            await _unitOfWork.MeterRepository.AddAsync(meterReading);
            await _unitOfWork.CommitAsync();

            return Shared.Response<MeterReadingDTO>.Success(_mapper.Map<MeterReadingDTO>(meterReading), 201);
        }
    }
}
