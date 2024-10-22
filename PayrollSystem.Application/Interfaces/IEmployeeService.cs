using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto employeeDto);
        Task<EmployeeDto> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto employeeDto);
        Task DeleteEmployeeAsync(int id);
        Task<EmployeePersonalInfoDto> GetEmployeePersonalInfoAsync(int employeeId);
        Task UpdateEmployeePersonalInfoAsync(int employeeId, EmployeePersonalInfoDto personalInfo);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<EmployeeDto> AssignEmployeeToDepartmentAsync(int employeeId, int departmentId);
        Task<EmployeeDto> UpdateEmployeeSalaryAsync(int employeeId, decimal newSalary);
        Task<EmployeeDto> UpdateEmployeeTaxBracketAsync(int employeeId, int newTaxBracketId);
        Task<EmployeeDto> UpdateEmployeeMedicalAidPlanAsync(int employeeId, int newMedicalAidPlanId);
        Task<IEnumerable<PayrollDto>> GetEmployeePayrollHistoryAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<bool> IsEmployeeActiveAsync(int employeeId);
        Task<EmployeeDto> TerminateEmployeeAsync(int employeeId, DateTime terminationDate);
        Task<EmployeeDto> ReinstateEmployeeAsync(int employeeId);
    }
}
