using FluentValidation;
using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Validators
{
    public class LeaveDtoValidator : AbstractValidator<LeaveDto>
    {
        public LeaveDtoValidator()
        {
            RuleFor(x => x.EmployeeId).GreaterThan(0);
            RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
            RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
            RuleFor(x => x.LeaveType).IsInEnum();
            RuleFor(x => x.Comments).MaximumLength(500);
        }
    }
}
