using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Interfaces
{
    public interface IEmployeeBenefitRepository
    {
        Task<EmployeeBenefit> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeBenefit>> GetByEmployeeIdAsync(int employeeId);
        Task AddAsync(EmployeeBenefit employeeBenefit);
        Task UpdateAsync(EmployeeBenefit employeeBenefit);
        Task DeleteAsync(int id);
    }
}
