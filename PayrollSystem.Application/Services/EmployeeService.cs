using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;
using FluentValidation;
using PayrollSystem.Application.Validators;

namespace PayrollSystem.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITaxBracketRepository _taxBracketRepository;
        private readonly IMedicalAidPlanRepository _medicalAidPlanRepository;
        private readonly IPayrollRepository _payrollRepository;
        private readonly CreateEmployeeDtoValidator _createEmployeeValidator;
        private readonly UpdateEmployeeDtoValidator _updateEmployeeValidator;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            ITaxBracketRepository taxBracketRepository,
            IMedicalAidPlanRepository medicalAidPlanRepository,
            IPayrollRepository payrollRepository,
            CreateEmployeeDtoValidator createEmployeeValidator,
            UpdateEmployeeDtoValidator updateEmployeeValidator)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _taxBracketRepository = taxBracketRepository;
            _medicalAidPlanRepository = medicalAidPlanRepository;
            _payrollRepository = payrollRepository;
            _createEmployeeValidator = createEmployeeValidator;
            _updateEmployeeValidator = updateEmployeeValidator;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto employeeDto)
        {
            var validationResult = await _createEmployeeValidator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                DateOfBirth = employeeDto.DateOfBirth,
                HireDate = employeeDto.HireDate,
                BaseSalary = employeeDto.BaseSalary,
                DepartmentId = employeeDto.DepartmentId,
                TaxBracketId = employeeDto.TaxBracketId,
                MedicalAidPlanId = employeeDto.MedicalAidPlanId,
                IsActive = true
            };

            await _employeeRepository.AddAsync(employee);
            return MapToDto(employee);
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {id} not found.");

            return MapToDto(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Select(MapToDto);
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto employeeDto)
        {
            var validationResult = await _updateEmployeeValidator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {id} not found.");

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Email = employeeDto.Email;
            //employee.DateOfBirth = employeeDto.DateOfBirth;
            //employee.HireDate = employeeDto.HireDate;
            employee.BaseSalary = employeeDto.BaseSalary;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.TaxBracketId = employeeDto.TaxBracketId;
            employee.MedicalAidPlanId = employeeDto.MedicalAidPlanId;

            await _employeeRepository.UpdateAsync(employee);
            return MapToDto(employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {id} not found.");

            await _employeeRepository.DeleteAsync(id);
        }

        public async Task<EmployeePersonalInfoDto> GetEmployeePersonalInfoAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            return new EmployeePersonalInfoDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DateOfBirth = employee.DateOfBirth,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                EmergencyContactName = employee.EmergencyContactName,
                EmergencyContactPhone = employee.EmergencyContactPhone
            };
        }

        public async Task UpdateEmployeePersonalInfoAsync(int employeeId, EmployeePersonalInfoDto personalInfo)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            employee.FirstName = personalInfo.FirstName;
            employee.LastName = personalInfo.LastName;
            employee.Email = personalInfo.Email;
            employee.DateOfBirth = personalInfo.DateOfBirth;
            employee.PhoneNumber = personalInfo.PhoneNumber;
            employee.Address = personalInfo.Address;
            employee.EmergencyContactName = personalInfo.EmergencyContactName;
            employee.EmergencyContactPhone = personalInfo.EmergencyContactPhone;

            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            var employees = await _employeeRepository.GetByDepartmentIdAsync(departmentId);
            return employees.Select(MapToDto);
        }

        public async Task<EmployeeDto> AssignEmployeeToDepartmentAsync(int employeeId, int departmentId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"Department with ID {departmentId} not found.");

            employee.DepartmentId = departmentId;
            await _employeeRepository.UpdateAsync(employee);
            return MapToDto(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeSalaryAsync(int employeeId, decimal newSalary)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            employee.BaseSalary = newSalary;
            await _employeeRepository.UpdateAsync(employee);
            return MapToDto(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeTaxBracketAsync(int employeeId, int newTaxBracketId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            var taxBracket = await _taxBracketRepository.GetByIdAsync(newTaxBracketId);
            if (taxBracket == null)
                throw new KeyNotFoundException($"Tax Bracket with ID {newTaxBracketId} not found.");

            employee.TaxBracketId = newTaxBracketId;
            await _employeeRepository.UpdateAsync(employee);
            return MapToDto(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeMedicalAidPlanAsync(int employeeId, int newMedicalAidPlanId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            var medicalAidPlan = await _medicalAidPlanRepository.GetByIdAsync(newMedicalAidPlanId);
            if (medicalAidPlan == null)
                throw new KeyNotFoundException($"Medical Aid Plan with ID {newMedicalAidPlanId} not found.");

            employee.MedicalAidPlanId = newMedicalAidPlanId;
            await _employeeRepository.UpdateAsync(employee);
            return MapToDto(employee);
        }

        public async Task<IEnumerable<PayrollDto>> GetEmployeePayrollHistoryAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var payrolls = await _payrollRepository.GetByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate);
            return payrolls.Select(MapToPayrollDto);
        }

        public async Task<bool> IsEmployeeActiveAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            return employee.IsActive;
        }

        public async Task<EmployeeDto> TerminateEmployeeAsync(int employeeId, DateTime terminationDate)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            employee.IsActive = false;
            employee.TerminationDate = terminationDate;
            await _employeeRepository.UpdateAsync(employee);
            return MapToDto(employee);
        }

        public async Task<EmployeeDto> ReinstateEmployeeAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            employee.IsActive = true;
            employee.TerminationDate = null;
            await _employeeRepository.UpdateAsync(employee);
            return MapToDto(employee);
        }

        private EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                BaseSalary = employee.BaseSalary,
                DepartmentId = employee.DepartmentId,
                TaxBracketId = employee.TaxBracketId,
                MedicalAidPlanId = employee.MedicalAidPlanId,
                IsActive = employee.IsActive,
                TerminationDate = employee.TerminationDate
            };
        }

        private PayrollDto MapToPayrollDto(Payroll payroll)
        {
            return new PayrollDto
            {
                Id = payroll.Id,
                EmployeeId = payroll.EmployeeId,
                PayPeriodStart = payroll.PayPeriodStart,
                PayPeriodEnd = payroll.PayPeriodEnd,
                GrossSalary = payroll.GrossSalary,
                TaxDeduction = payroll.TaxDeduction,
                MedicalAidDeduction = payroll.MedicalAidDeduction,
                NetSalary = payroll.NetSalary
            };
        }
    }
}
