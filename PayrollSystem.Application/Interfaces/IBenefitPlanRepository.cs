using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Interfaces
{
    public interface IBenefitPlanRepository
    {
        Task<BenefitPlan> GetByIdAsync(int id);
        Task<IEnumerable<BenefitPlan>> GetAllAsync();
        Task AddAsync(BenefitPlan benefitPlan);
        Task UpdateAsync(BenefitPlan benefitPlan);
        Task DeleteAsync(int id);
    }
}
