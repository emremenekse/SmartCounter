namespace CounterService.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        IMeterRepository MeterRepository { get; }
        Task<int> CommitAsync();
    }
}
