using CounterService.Entity;

namespace CounterService.Abstraction
{
    public interface IMeterRepository
    {
        Task<IEnumerable<MeterReading>> GetAllAsync();
        Task<MeterReading> GetBySerialNumberAsync(string serialNumber);
        Task AddAsync(MeterReading meterReading);
    }
}
