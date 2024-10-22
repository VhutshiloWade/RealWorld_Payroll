using FluentValidation;
using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Validators
{
    public class PayrollCalculationRequestDtoValidator : AbstractValidator<PayrollCalculationRequestDto>
    {
        public PayrollCalculationRequestDtoValidator()
        {
            RuleFor(x => x.EmployeeId).GreaterThan(0);
            RuleFor(x => x.PayPeriodStart).NotEmpty().LessThan(x => x.PayPeriodEnd);
            RuleFor(x => x.PayPeriodEnd).NotEmpty().GreaterThan(x => x.PayPeriodStart);
            RuleFor(x => x.OvertimeHours).GreaterThanOrEqualTo(0);
            RuleFor(x => x.BonusPercentage).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
        }
    }
}
