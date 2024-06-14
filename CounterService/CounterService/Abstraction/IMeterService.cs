using CounterService.DTOs;
using CounterService.Entity;

namespace CounterService.Abstraction
{
    public interface IMeterService
    {
        Task<IEnumerable<MeterReadingDTO>> GetAllAsync();
        Task<MeterReadingDTO> GetBySerialNumberAsync(string serialNumber);
        Task AddAsync(MeterReadingDTO meterReading);
    }
}
