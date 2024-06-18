using AutoMapper;
using CounterService.Abstraction;
using CounterService.DTOs;
using CounterService.Entity;
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

        public async Task<Shared.Response<MeterReadingDTO>> GetBySerialNumberAsync(string serialNumber)
        {
            var meterReading = await _unitOfWork.MeterRepository.GetBySerialNumberAsync(serialNumber);
            return Shared.Response < MeterReadingDTO>.Success( _mapper.Map<MeterReadingDTO>(meterReading),200);
        }

        public async Task<Shared.Response<MeterReadingDTO>> AddAsync(MeterReadingDTO meterReadingDto)
        {
            meterReadingDto.Id = Guid.NewGuid();
            var meterReading = _mapper.Map<MeterReading>(meterReadingDto);
            await _unitOfWork.MeterRepository.AddAsync(meterReading);
            await _unitOfWork.CommitAsync();

            return Shared.Response<MeterReadingDTO>.Success(_mapper.Map<MeterReadingDTO>(meterReading), 201);
        }
    }
}
