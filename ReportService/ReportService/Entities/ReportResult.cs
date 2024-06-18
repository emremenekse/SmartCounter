namespace ReportService.Entities
{
    public class ReportResult
    {
        public Guid Id { get; set; }
        public Guid ReportRequestId { get; set; }
        public string FilePath { get; set; }
        public DateTime GeneratedTime { get; set; }
    }
}
