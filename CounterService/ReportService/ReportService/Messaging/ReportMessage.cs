namespace ReportService.Messaging
{
    public class ReportMessage
    {
        public Guid ReportRequestId { get; set; }
        public string SerialNumber { get; set; }
        public double LastIndex { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
        public string MeasurementTime { get; set; }
    }

}
