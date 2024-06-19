using Microsoft.EntityFrameworkCore;
using ReportService.Abstractions;
using ReportService.Data;
using ReportService.Entities;
using System;

namespace ReportService.Concrete
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportContext _context;

        public ReportRepository(ReportContext context)
        {
            _context = context;
        }

        public async Task AddRequestAsync(ReportRequest request)
        {
            await _context.ReportRequests.AddAsync(request);
        }

        public async Task UpdateRequestStatusAsync(Guid requestId, string status)
        {
            var request = await _context.ReportRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = status;
            }
        }

        public async Task AddResultAsync(ReportResult result)
        {
            await _context.ReportResults.AddAsync(result);
        }

        public async Task<IEnumerable<ReportRequest>> GetAllRequestsAsync()
        {
            return await _context.ReportRequests.ToListAsync();
        }

        public async Task<ReportRequest> GetRequestByIdAsync(Guid requestId)
        {
            return await _context.ReportRequests.FindAsync(requestId);
        }
        public async Task<ReportRequest> GetRequestBySerialNumberAsync(string serialNumber)
        {
            return await _context.ReportRequests.FirstOrDefaultAsync(r => r.SerialNumber == serialNumber);
        }
        public async Task<ReportRequest> GetRequestBySerialNumberAsyncWithDate(string serialNumber, DateTime DateTime)
        {
            var utcDateTime = DateTime.SpecifyKind(DateTime, DateTimeKind.Utc);
            return await _context.ReportRequests.FirstOrDefaultAsync(r => r.SerialNumber == serialNumber && r.MeasurementTime == utcDateTime);
        }

        public async Task<ReportResult> GetResultByRequestIdAsync(Guid requestId)
        {
            return await _context.ReportResults.FirstOrDefaultAsync(r => r.ReportRequestId == requestId);
        }
    }
}
