using Microsoft.EntityFrameworkCore;
using ReportService.Entities;
using System.Collections.Generic;

namespace ReportService.Data
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options) { }

        public DbSet<ReportRequest> ReportRequests { get; set; }
        public DbSet<ReportResult> ReportResults { get; set; }
    }
}
