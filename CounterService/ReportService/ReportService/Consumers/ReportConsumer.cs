using MassTransit;
using ReportService.Abstractions;
using ReportService.Entities;
using ReportService.Messaging;
using System.Text.Json;

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
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var reportRepository = scope.ServiceProvider.GetRequiredService<IUnitOfWork>().ReportRepository;
                try
                {
                    await reportRepository.UpdateRequestStatusAsync(reportMessage.ReportRequestId, "Processing");

                    var reportData = $"Report for Serial Number: {reportMessage.SerialNumber}\n" +
                                     $"Last Index: {reportMessage.LastIndex}\n" +
                                     $"Voltage: {reportMessage.Voltage}\n" +
                                     $"Current: {reportMessage.Current}\n" +
                                     $"Measurement Time: {reportMessage.MeasurementTime}";

                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var filePath = Path.Combine(directoryPath, $"{reportMessage.ReportRequestId}.txt");
                    var content = GetReportContent(reportMessage, "txt"); 

                    await File.WriteAllTextAsync(filePath, content);
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
                    await unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
                


            }
        }
        private string GetReportContent(ReportMessage reportMessage, string format)
        {
            if (format.ToLower() == "json")
            {
                return JsonSerializer.Serialize(reportMessage);
            }
            else 
            {
                return $"Report for Serial Number: {reportMessage.SerialNumber}\n" +
                       $"Last Index: {reportMessage.LastIndex}\n" +
                       $"Voltage: {reportMessage.Voltage}\n" +
                       $"Current: {reportMessage.Current}\n" +
                       $"Measurement Time: {reportMessage.MeasurementTime}";
            }
        }
    }
}
