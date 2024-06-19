using ReportService.Abstractions;
using ReportService.Data;

namespace ReportService.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReportContext _context;
        private IReportRepository _reportRepository;

        public UnitOfWork(ReportContext context)
        {
            _context = context;
        }

        public IReportRepository ReportRepository
        {
            get
            {
                return _reportRepository ??= new ReportRepository(_context);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
