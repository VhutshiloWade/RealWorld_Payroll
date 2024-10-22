using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Interfaces
{
    public interface IBenefitsService
    {
        Task<BenefitPlanDto> CreateBenefitPlanAsync(BenefitPlanDto benefitPlanDto);
        Task<BenefitPlanDto> UpdateBenefitPlanAsync(int id, BenefitPlanDto benefitPlanDto);
        Task DeleteBenefitPlanAsync(int id);
        Task<BenefitPlanDto> GetBenefitPlanByIdAsync(int id);
        Task<IEnumerable<BenefitPlanDto>> GetAllBenefitPlansAsync();
        Task<EmployeeBenefitDto> EnrollEmployeeInBenefitPlanAsync(int employeeId, int benefitPlanId);
        Task<EmployeeBenefitDto> TerminateEmployeeBenefitAsync(int employeeBenefitId);
        Task<IEnumerable<EmployeeBenefitDto>> GetEmployeeBenefitsAsync(int employeeId);
        Task<decimal> CalculateTotalBenefitsCostAsync(int employeeId);
    }
}
