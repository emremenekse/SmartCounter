using AutoMapper;
using CounterService.Abstraction;
using CounterService.DTOs;
using CounterService.Entity;

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

        public async Task<IEnumerable<MeterReadingDTO>> GetAllAsync()
        {
            var meterReadings = await _unitOfWork.MeterRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MeterReadingDTO>>(meterReadings);
        }

        public async Task<MeterReadingDTO> GetBySerialNumberAsync(string serialNumber)
        {
            var meterReading = await _unitOfWork.MeterRepository.GetBySerialNumberAsync(serialNumber);
            return _mapper.Map<MeterReadingDTO>(meterReading);
        }

        public async Task AddAsync(MeterReadingDTO meterReadingDto)
        {
            meterReadingDto.Id = Guid.NewGuid();
            var meterReading = _mapper.Map<MeterReading>(meterReadingDto);
            await _unitOfWork.MeterRepository.AddAsync(meterReading);
            await _unitOfWork.CommitAsync();
        }
    }
}
