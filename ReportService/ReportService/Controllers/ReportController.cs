using Microsoft.AspNetCore.Mvc;
using ReportService.Abstractions;
using ReportService.DTOs;

namespace ReportService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : CustomBaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReportRequest([FromBody] string serialNumber)
        {
            var reportRequest = await _reportService.CreateReportRequestAsync(serialNumber);
            return CreateActionResultInstance(reportRequest);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReportRequests()
        {
            var reportRequests = await _reportService.GetAllReportRequestsAsync();
            return CreateActionResultInstance(reportRequests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportRequestById(Guid id)
        {
            var reportRequest = await _reportService.GetReportRequestByIdAsync(id);
            if (reportRequest == null)
            {
                return NotFound();
            }
            return CreateActionResultInstance(reportRequest);
        }
    }
}
