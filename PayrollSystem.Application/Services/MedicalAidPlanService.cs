using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Services
{
    public class MedicalAidPlanService : IMedicalAidPlanService
    {
        private readonly IMedicalAidPlanRepository _medicalAidPlanRepository;

        public MedicalAidPlanService(IMedicalAidPlanRepository medicalAidPlanRepository)
        {
            _medicalAidPlanRepository = medicalAidPlanRepository;
        }

        public async Task<IEnumerable<MedicalAidPlanDto>> GetAllMedicalAidPlansAsync()
        {
            var medicalAidPlans = await _medicalAidPlanRepository.GetAllAsync();
            return medicalAidPlans.Select(m => MapToDto(m));
        }

        public async Task<MedicalAidPlanDto> GetMedicalAidPlanByIdAsync(int id)
        {
            var medicalAidPlan = await _medicalAidPlanRepository.GetByIdAsync(id);
            return medicalAidPlan != null ? MapToDto(medicalAidPlan) : null;
        }

        public async Task<MedicalAidPlanDto> CreateMedicalAidPlanAsync(MedicalAidPlanDto medicalAidPlanDto)
        {
            var medicalAidPlan = MapToEntity(medicalAidPlanDto);
            await _medicalAidPlanRepository.AddAsync(medicalAidPlan);
            return MapToDto(medicalAidPlan);
        }

        public async Task UpdateMedicalAidPlanAsync(int id, MedicalAidPlanDto medicalAidPlanDto)
        {
            var medicalAidPlan = await _medicalAidPlanRepository.GetByIdAsync(id);
            if (medicalAidPlan == null)
            {
                throw new KeyNotFoundException($"Medical Aid Plan with ID {id} not found.");
            }

            UpdateEntityFromDto(medicalAidPlan, medicalAidPlanDto);
            await _medicalAidPlanRepository.UpdateAsync(medicalAidPlan);
        }

        public async Task DeleteMedicalAidPlanAsync(int id)
        {
            await _medicalAidPlanRepository.DeleteAsync(id);
        }

        private MedicalAidPlanDto MapToDto(MedicalAidPlan medicalAidPlan)
        {
            return new MedicalAidPlanDto
            {
                Id = medicalAidPlan.Id,
                Name = medicalAidPlan.Name,
                MonthlyPremium = medicalAidPlan.MonthlyPremium
            };
        }

        private MedicalAidPlan MapToEntity(MedicalAidPlanDto dto)
        {
            return new MedicalAidPlan
            {
                Name = dto.Name,
                MonthlyPremium = dto.MonthlyPremium
            };
        }

        private void UpdateEntityFromDto(MedicalAidPlan medicalAidPlan, MedicalAidPlanDto dto)
        {
            medicalAidPlan.Name = dto.Name;
            medicalAidPlan.MonthlyPremium = dto.MonthlyPremium;
        }
    }
}
