namespace CounterService.Entity
{
    public class MeterReading
    {
        public Guid Id { get; set; }
        public required string SerialNumber { get; set; }
        public string MeasurementTime { get; set; }
        public decimal LastIndex { get; set; }
        public decimal Voltage { get; set; }
        public decimal Current { get; set; }
    }
}
