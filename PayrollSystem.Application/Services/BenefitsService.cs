using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Services
{
    public class BenefitsService : IBenefitsService
    {
        private readonly IBenefitPlanRepository _benefitPlanRepository;
        private readonly IEmployeeBenefitRepository _employeeBenefitRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public BenefitsService(
            IBenefitPlanRepository benefitPlanRepository,
            IEmployeeBenefitRepository employeeBenefitRepository,
            IEmployeeRepository employeeRepository)
        {
            _benefitPlanRepository = benefitPlanRepository;
            _employeeBenefitRepository = employeeBenefitRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<BenefitPlanDto> CreateBenefitPlanAsync(BenefitPlanDto benefitPlanDto)
        {
            var benefitPlan = new BenefitPlan
            {
                Name = benefitPlanDto.Name,
                Description = benefitPlanDto.Description,
                Cost = benefitPlanDto.Cost,
                Type = benefitPlanDto.Type
            };

            await _benefitPlanRepository.AddAsync(benefitPlan);
            return MapToBenefitPlanDto(benefitPlan);
        }

        public async Task<BenefitPlanDto> UpdateBenefitPlanAsync(int id, BenefitPlanDto benefitPlanDto)
        {
            var benefitPlan = await _benefitPlanRepository.GetByIdAsync(id);
            if (benefitPlan == null)
                throw new KeyNotFoundException($"Benefit plan with ID {id} not found.");

            benefitPlan.Name = benefitPlanDto.Name;
            benefitPlan.Description = benefitPlanDto.Description;
            benefitPlan.Cost = benefitPlanDto.Cost;
            benefitPlan.Type = benefitPlanDto.Type;

            await _benefitPlanRepository.UpdateAsync(benefitPlan);
            return MapToBenefitPlanDto(benefitPlan);
        }

        public async Task DeleteBenefitPlanAsync(int id)
        {
            await _benefitPlanRepository.DeleteAsync(id);
        }

        public async Task<BenefitPlanDto> GetBenefitPlanByIdAsync(int id)
        {
            var benefitPlan = await _benefitPlanRepository.GetByIdAsync(id);
            if (benefitPlan == null)
                throw new KeyNotFoundException($"Benefit plan with ID {id} not found.");

            return MapToBenefitPlanDto(benefitPlan);
        }

        public async Task<IEnumerable<BenefitPlanDto>> GetAllBenefitPlansAsync()
        {
            var benefitPlans = await _benefitPlanRepository.GetAllAsync();
            return benefitPlans.Select(MapToBenefitPlanDto);
        }

        public async Task<EmployeeBenefitDto> EnrollEmployeeInBenefitPlanAsync(int employeeId, int benefitPlanId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            var benefitPlan = await _benefitPlanRepository.GetByIdAsync(benefitPlanId);
            if (benefitPlan == null)
                throw new KeyNotFoundException($"Benefit plan with ID {benefitPlanId} not found.");

            var employeeBenefit = new EmployeeBenefit
            {
                EmployeeId = employeeId,
                BenefitPlanId = benefitPlanId,
                EnrollmentDate = DateTime.UtcNow
            };

            await _employeeBenefitRepository.AddAsync(employeeBenefit);
            return MapToEmployeeBenefitDto(employeeBenefit, benefitPlan.Name);
        }

        public async Task<EmployeeBenefitDto> TerminateEmployeeBenefitAsync(int employeeBenefitId)
        {
            var employeeBenefit = await _employeeBenefitRepository.GetByIdAsync(employeeBenefitId);
            if (employeeBenefit == null)
                throw new KeyNotFoundException($"Employee benefit with ID {employeeBenefitId} not found.");

            employeeBenefit.TerminationDate = DateTime.UtcNow;
            await _employeeBenefitRepository.UpdateAsync(employeeBenefit);

            var benefitPlan = await _benefitPlanRepository.GetByIdAsync(employeeBenefit.BenefitPlanId);
            return MapToEmployeeBenefitDto(employeeBenefit, benefitPlan.Name);
        }

        public async Task<IEnumerable<EmployeeBenefitDto>> GetEmployeeBenefitsAsync(int employeeId)
        {
            var employeeBenefits = await _employeeBenefitRepository.GetByEmployeeIdAsync(employeeId);
            var benefitPlans = await _benefitPlanRepository.GetAllAsync();
            var benefitPlanDict = benefitPlans.ToDictionary(bp => bp.Id, bp => bp.Name);

            return employeeBenefits.Select(eb => MapToEmployeeBenefitDto(eb, benefitPlanDict[eb.BenefitPlanId]));
        }

        public async Task<decimal> CalculateTotalBenefitsCostAsync(int employeeId)
        {
            var employeeBenefits = await _employeeBenefitRepository.GetByEmployeeIdAsync(employeeId);
            var activeBenefits = employeeBenefits.Where(eb => eb.TerminationDate == null || eb.TerminationDate > DateTime.UtcNow);
            
            decimal totalCost = 0;
            foreach (var benefit in activeBenefits)
            {
                var benefitPlan = await _benefitPlanRepository.GetByIdAsync(benefit.BenefitPlanId);
                totalCost += benefitPlan.Cost;
            }

            return totalCost;
        }

        private BenefitPlanDto MapToBenefitPlanDto(BenefitPlan benefitPlan)
        {
            return new BenefitPlanDto
            {
                Id = benefitPlan.Id,
                Name = benefitPlan.Name,
                Description = benefitPlan.Description,
                Cost = benefitPlan.Cost,
                Type = benefitPlan.Type
            };
        }

        private EmployeeBenefitDto MapToEmployeeBenefitDto(EmployeeBenefit employeeBenefit, string benefitPlanName)
        {
            return new EmployeeBenefitDto
            {
                Id = employeeBenefit.Id,
                EmployeeId = employeeBenefit.EmployeeId,
                BenefitPlanId = employeeBenefit.BenefitPlanId,
                BenefitPlanName = benefitPlanName,
                EnrollmentDate = employeeBenefit.EnrollmentDate,
                TerminationDate = employeeBenefit.TerminationDate
            };
        }
    }
}
