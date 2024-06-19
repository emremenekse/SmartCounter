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
            var meterReading = await _meterService.GetBySerialNumberAndMeasurementTimeAsync(request.SerialNumber, DateTime.Parse(request.MeasurementTime));
            if (meterReading.Data == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Meter reading not found."));
            }
            var response = new CounterResponse
            {
                SerialNumber = request.SerialNumber,
                LastIndex = (double)meterReading.Data.LastIndex, 
                Voltage = (double)meterReading.Data.Voltage, 
                Current = (double)meterReading.Data.Current, 
                MeasurementTime = request.MeasurementTime
            };

            return response;
        }
    }
}
