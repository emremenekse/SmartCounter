using CounterService.Abstraction;
using CounterService.Data;
using CounterService.Entity;
using Microsoft.EntityFrameworkCore;

namespace CounterService.Repository
{
    public class MeterRepository : IMeterRepository
    {
        private readonly MeterContext _context;

        public MeterRepository(MeterContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MeterReading>> GetAllAsync()
        {
            return await _context.MeterReadings.ToListAsync();
        }

        public async Task<MeterReading> GetBySerialNumberAsync(string serialNumber)
        {
            return await _context.MeterReadings
                .Where(m => m.SerialNumber == serialNumber)
                .OrderByDescending(m => m.MeasurementTime)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(MeterReading meterReading)
        {
            await _context.MeterReadings.AddAsync(meterReading);
        }
    }
}
