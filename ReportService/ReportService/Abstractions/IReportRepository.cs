using ReportService.Entities;

namespace ReportService.Abstractions
{
    public interface IReportRepository
    {
        Task AddRequestAsync(ReportRequest request);
        Task UpdateRequestStatusAsync(Guid requestId, string status);
        Task AddResultAsync(ReportResult result);
        Task<IEnumerable<ReportRequest>> GetAllRequestsAsync();
        Task<ReportRequest> GetRequestByIdAsync(Guid requestId);
        Task<ReportResult> GetResultByRequestIdAsync(Guid requestId);
    }
}
