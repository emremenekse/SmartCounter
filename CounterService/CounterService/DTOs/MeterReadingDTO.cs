namespace CounterService.DTOs
{
    public class MeterReadingDTO
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime MeasurementTime { get; set; }
        public decimal LastIndex { get; set; }
        public decimal Voltage { get; set; }
        public decimal Current { get; set; }
    }
}
