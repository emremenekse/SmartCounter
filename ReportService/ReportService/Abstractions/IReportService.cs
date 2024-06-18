using ReportService.DTOs;

namespace ReportService.Abstractions
{
    public interface IReportService
    {
        Task<ReportRequestDTO> CreateReportRequestAsync(string serialNumber);
        Task<IEnumerable<ReportRequestDTO>> GetAllReportRequestsAsync();
        Task<ReportRequestDTO> GetReportRequestByIdAsync(Guid requestId);
    }
}
