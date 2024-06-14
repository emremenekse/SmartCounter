using CounterService.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CounterService.Data
{
    public class MeterContext : DbContext
    {
        public MeterContext(DbContextOptions<MeterContext> options) : base(options) { }

        public DbSet<MeterReading> MeterReadings { get; set; }
    }
}
