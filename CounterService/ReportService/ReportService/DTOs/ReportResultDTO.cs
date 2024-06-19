namespace ReportService.DTOs
{
    public class ReportResultDTO
    {
        public Guid Id { get; set; }
        public Guid ReportRequestId { get; set; }
        public string FilePath { get; set; }
        public DateTime GeneratedTime { get; set; }
    }
}
