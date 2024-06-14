using CounterService.DTOs;
using FluentValidation;

namespace CounterService.Validation
{
    public class MeterReadingDTOValidator : AbstractValidator<MeterReadingDTO>
    {
        public MeterReadingDTOValidator()
        {
            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage("Serial number is required.")
                .Length(8).WithMessage("Serial number must be 8 characters long.");

            RuleFor(x => x.MeasurementTime)
                .NotEmpty().WithMessage("Measurement time is required.");

            RuleFor(x => x.LastIndex)
                .GreaterThanOrEqualTo(0).WithMessage("Last index must be greater than or equal to 0.");

            RuleFor(x => x.Voltage)
                .GreaterThan(0).WithMessage("Voltage must be greater than 0.");

            RuleFor(x => x.Current)
                .GreaterThan(0).WithMessage("Current must be greater than 0.");
        }
    }
}
