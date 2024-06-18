﻿using ReportService.DTOs;

namespace ReportService.Abstractions
{
    public interface IReportService
    {
        Task<Shared.Response<ReportRequestDTO>> CreateReportRequestAsync(string serialNumber);
        Task<Shared.Response<IEnumerable<ReportRequestDTO>>> GetAllReportRequestsAsync();
        Task<Shared.Response<ReportRequestDTO>> GetReportRequestByIdAsync(Guid requestId);
    }
}
