using CounterService.Abstraction;
using CounterService.DTOs;
using CounterService.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CounterService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MeterController : ControllerBase
    {
        private readonly IMeterService _meterService;

        public MeterController(IMeterService meterService)
        {
            _meterService = meterService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterReadingDTO>>> GetAll()
        {
            var meterReadings = await _meterService.GetAllAsync();
            return Ok(meterReadings);
        }

        [HttpGet("{serialNumber}")]
        public async Task<ActionResult<MeterReadingDTO>> GetBySerialNumber(string serialNumber)
        {
            var meterReading = await _meterService.GetBySerialNumberAsync(serialNumber);
            if (meterReading == null)
            {
                return NotFound();
            }
            return Ok(meterReading);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] MeterReadingDTO meterReadingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _meterService.AddAsync(meterReadingDto);
            return CreatedAtAction(nameof(GetBySerialNumber), new { serialNumber = meterReadingDto.SerialNumber }, meterReadingDto);
        }
    }
}
