using CounterService.Entity;

namespace CounterService.Abstraction
{
    public interface IMeterRepository
    {
        Task<IEnumerable<MeterReading>> GetAllAsync();
        Task<MeterReading> GetBySerialNumberAndMeasurementTimeAsync(string serialNumber,DateTime dateTime);
        Task AddAsync(MeterReading meterReading);
    }
}
