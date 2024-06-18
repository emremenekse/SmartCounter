using Grpc.Core;
using System.Threading.Tasks;
using CounterService.Grpc;
using CounterService.Abstraction;

namespace CounterService.Service
{
    public class CounterGrpcService : Counter.CounterBase
    {
        private readonly IMeterService _meterService;

        public CounterGrpcService(IMeterService meterService)
        {
            _meterService = meterService;
        }

        public override async  Task<CounterResponse> GetCounterData(CounterRequest request, ServerCallContext context)
        {
            var meterReading = await _meterService.GetBySerialNumberAsync(request.SerialNumber);

            var response = new CounterResponse
            {
                SerialNumber = request.SerialNumber,
                LastIndex = (double)meterReading.LastIndex, 
                Voltage = (double)meterReading.Voltage, 
                Current = (double)meterReading.Current, 
                MeasurementTime = meterReading.MeasurementTime.ToString()
            };

            return response;
        }
    }
}
