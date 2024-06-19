namespace ReportService.DTOs
{
    public class ReportRequestDTO
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string FilePath { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime MeasurementTime { get; set; }


    }
}
