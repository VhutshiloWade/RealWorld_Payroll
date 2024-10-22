using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Interfaces
{
    public interface IMedicalAidPlanService
    {
        Task<IEnumerable<MedicalAidPlanDto>> GetAllMedicalAidPlansAsync();
        Task<MedicalAidPlanDto> GetMedicalAidPlanByIdAsync(int id);
        Task<MedicalAidPlanDto> CreateMedicalAidPlanAsync(MedicalAidPlanDto medicalAidPlanDto);
        Task UpdateMedicalAidPlanAsync(int id, MedicalAidPlanDto medicalAidPlanDto);
        Task DeleteMedicalAidPlanAsync(int id);
    }
}
