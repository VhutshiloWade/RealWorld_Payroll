using FluentValidation;
using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Validators
{
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(x => x.DateOfBirth).NotEmpty().LessThan(DateTime.Now.AddYears(-18));
            RuleFor(x => x.HireDate).NotEmpty().LessThanOrEqualTo(DateTime.Now);
            RuleFor(x => x.BaseSalary).GreaterThan(0);
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.TaxBracketId).GreaterThan(0);
            RuleFor(x => x.MedicalAidPlanId).GreaterThan(0);
        }
    }
}
