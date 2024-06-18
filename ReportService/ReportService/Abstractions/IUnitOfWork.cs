namespace ReportService.Abstractions
{
    public interface IUnitOfWork
    {
        IReportRepository ReportRepository { get; }
        Task SaveChangesAsync();
    }
}
