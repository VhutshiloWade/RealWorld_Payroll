using PayrollSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Interfaces
{
    public interface IMedicalAidPlanRepository
    {
        Task<IEnumerable<MedicalAidPlan>> GetAllAsync();
        Task<MedicalAidPlan> GetByIdAsync(int id);
        Task AddAsync(MedicalAidPlan medicalAidPlan);
        Task UpdateAsync(MedicalAidPlan medicalAidPlan);
        Task DeleteAsync(int id);
    }
}
