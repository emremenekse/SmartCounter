using CounterService.Abstraction;
using CounterService.DTOs;
using CounterService.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CounterService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MeterController : CustomBaseController
    {
        private readonly IMeterService _meterService;

        public MeterController(IMeterService meterService)
        {
            _meterService = meterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var meterReadings = await _meterService.GetAllAsync();
            return CreateActionResultInstance(meterReadings);
        }

        [HttpGet("{serialNumber}/{measurementTime}")]
        public async Task<IActionResult> GetBySerialNumberAndMeasurementTime(string serialNumber, DateTime measurementTime)
        {
            var meterReading = await _meterService.GetBySerialNumberAndMeasurementTimeAsync(serialNumber, measurementTime);
            if (meterReading == null)
            {
                return NotFound();
            }
            return CreateActionResultInstance(meterReading);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MeterReadingDTO meterReadingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _meterService.AddAsync(meterReadingDto);
            return CreateActionResultInstance(result);
        }
    }
}
