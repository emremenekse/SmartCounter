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
        public async Task<IActionResult> CreateReportRequest([FromBody] CreateReportRequestDTO request)
        {
            var reportRequest = await _reportService.CreateReportRequestAsync(request.SerialNumber, request.MeasurementTime);
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

        [HttpGet("{filePath}")]
        public IActionResult DownloadFile(string filePath)
        {
            try
            {
                var file = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                var net = new System.Net.WebClient();
                var data = net.DownloadData(file);
                var content = new System.IO.MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";
                var fileName = Path.GetFileName(file);
                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
