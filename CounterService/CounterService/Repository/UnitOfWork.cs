using CounterService.Abstraction;
using CounterService.Data;

namespace CounterService.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MeterContext _context;
        private IMeterRepository _meterRepository;

        public UnitOfWork(MeterContext context)
        {
            _context = context;
        }

        public IMeterRepository MeterRepository
        {
            get
            {
                return _meterRepository ??= new MeterRepository(_context);
            }
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
