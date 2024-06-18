using Microsoft.AspNetCore.Mvc;
using ReportService.Abstractions;
using ReportService.DTOs;

namespace ReportService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<ActionResult<ReportRequestDTO>> CreateReportRequest([FromBody] string serialNumber)
        {
            var reportRequest = await _reportService.CreateReportRequestAsync(serialNumber);
            return CreatedAtAction(nameof(GetReportRequestById), new { id = reportRequest.Id }, reportRequest);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportRequestDTO>>> GetAllReportRequests()
        {
            var reportRequests = await _reportService.GetAllReportRequestsAsync();
            return Ok(reportRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportRequestDTO>> GetReportRequestById(Guid id)
        {
            var reportRequest = await _reportService.GetReportRequestByIdAsync(id);
            if (reportRequest == null)
            {
                return NotFound();
            }
            return Ok(reportRequest);
        }
    }
}
