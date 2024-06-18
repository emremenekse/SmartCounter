using FluentValidation;
using ReportService.DTOs;

namespace ReportService.Validation
{
    public class ReportRequestDTOValidator : AbstractValidator<ReportRequestDTO>
    {
        public ReportRequestDTOValidator()
        {
            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage("Serial number is required.")
                .Length(8).WithMessage("Serial number must be 8 characters long.");

            RuleFor(x => x.RequestTime)
                .NotEmpty().WithMessage("Request time is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => status == "Pending" || status == "Processing" || status == "Completed")
                .WithMessage("Status must be 'Pending', 'Processing' or 'Completed'.");
        }
    }
}
