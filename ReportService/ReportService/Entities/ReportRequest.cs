namespace ReportService.Entities
{
    public class ReportRequest
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime RequestTime { get; set; }
        public string Status { get; set; } 
    }
}
