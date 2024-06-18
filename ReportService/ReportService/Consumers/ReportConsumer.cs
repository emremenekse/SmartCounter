using MassTransit;
using ReportService.Abstractions;
using ReportService.Entities;
using ReportService.Messaging;

namespace ReportService.Consumers
{
    public class ReportConsumer : IConsumer<ReportMessage>
    {
        private readonly IServiceProvider _serviceProvider;

        public ReportConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<ReportMessage> context)
        {
            var reportMessage = context.Message;

            using (var scope = _serviceProvider.CreateScope())
            {
                var reportRepository = scope.ServiceProvider.GetRequiredService<IReportRepository>();

                await reportRepository.UpdateRequestStatusAsync(reportMessage.ReportRequestId, "Processing");

                var reportData = $"Report for Serial Number: {reportMessage.SerialNumber}\n" +
                                 $"Last Index: {reportMessage.LastIndex}\n" +
                                 $"Voltage: {reportMessage.Voltage}\n" +
                                 $"Current: {reportMessage.Current}\n" +
                                 $"Measurement Time: {reportMessage.MeasurementTime}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", $"{reportMessage.ReportRequestId}.txt");
                await File.WriteAllTextAsync(filePath, reportData);

                var reportResult = new ReportResult
                {
                    Id = Guid.NewGuid(),
                    ReportRequestId = reportMessage.ReportRequestId,
                    FilePath = filePath,
                    GeneratedTime = DateTime.UtcNow
                };
                await reportRepository.AddResultAsync(reportResult);

                await reportRepository.UpdateRequestStatusAsync(reportMessage.ReportRequestId, "Completed");
            }
        }
    }
}
