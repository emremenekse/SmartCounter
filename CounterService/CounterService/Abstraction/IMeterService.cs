using CounterService.DTOs;
using CounterService.Entity;

namespace CounterService.Abstraction
{
    public interface IMeterService
    {
        Task<Shared.Response<IEnumerable<MeterReadingDTO>>> GetAllAsync();
        Task<Shared.Response<MeterReadingDTO>> GetBySerialNumberAndMeasurementTimeAsync(string serialNumber,DateTime measurementTime);
        Task<Shared.Response<MeterReadingDTO>> AddAsync(MeterReadingDTO meterReading);
    }
}
