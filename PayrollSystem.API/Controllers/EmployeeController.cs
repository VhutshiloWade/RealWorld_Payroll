using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using FluentValidation;

namespace PayrollSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto employeeDto)
        {
            try
            {
                var result = await _employeeService.CreateEmployeeAsync(employeeDto);
                return CreatedAtAction(nameof(GetEmployee), new { id = result.Id }, result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                return Ok(employee);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto employeeDto)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, employeeDto);
                return Ok(updatedEmployee);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/personal-info")]
        public async Task<ActionResult<EmployeePersonalInfoDto>> GetEmployeePersonalInfo(int id)
        {
            try
            {
                var personalInfo = await _employeeService.GetEmployeePersonalInfoAsync(id);
                return Ok(personalInfo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/personal-info")]
        public async Task<IActionResult> UpdateEmployeePersonalInfo(int id, EmployeePersonalInfoDto personalInfo)
        {
            try
            {
                await _employeeService.UpdateEmployeePersonalInfoAsync(id, personalInfo);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(int departmentId)
        {
            var employees = await _employeeService.GetEmployeesByDepartmentAsync(departmentId);
            return Ok(employees);
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPut("{id}/department")]
        public async Task<ActionResult<EmployeeDto>> AssignEmployeeToDepartment(int id, [FromBody] int departmentId)
        {
            try
            {
                var updatedEmployee = await _employeeService.AssignEmployeeToDepartmentAsync(id, departmentId);
                return Ok(updatedEmployee);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPut("{id}/salary")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeSalary(int id, [FromBody] decimal newSalary)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeSalaryAsync(id, newSalary);
                return Ok(updatedEmployee);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPut("{id}/tax-bracket")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeTaxBracket(int id, [FromBody] int newTaxBracketId)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeTaxBracketAsync(id, newTaxBracketId);
                return Ok(updatedEmployee);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPut("{id}/medical-aid-plan")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeMedicalAidPlan(int id, [FromBody] int newMedicalAidPlanId)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeMedicalAidPlanAsync(id, newMedicalAidPlanId);
                return Ok(updatedEmployee);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/payroll-history")]
        public async Task<ActionResult<IEnumerable<PayrollDto>>> GetEmployeePayrollHistory(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var payrollHistory = await _employeeService.GetEmployeePayrollHistoryAsync(id, startDate, endDate);
                return Ok(payrollHistory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpGet("{id}/is-active")]
        public async Task<ActionResult<bool>> IsEmployeeActive(int id)
        {
            try
            {
                var isActive = await _employeeService.IsEmployeeActiveAsync(id);
                return Ok(isActive);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPut("{id}/terminate")]
        public async Task<ActionResult<EmployeeDto>> TerminateEmployee(int id, [FromBody] DateTime terminationDate)
        {
            try
            {
                var terminatedEmployee = await _employeeService.TerminateEmployeeAsync(id, terminationDate);
                return Ok(terminatedEmployee);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPut("{id}/reinstate")]
        public async Task<ActionResult<EmployeeDto>> ReinstateEmployee(int id)
        {
            try
            {
                var reinstatedEmployee = await _employeeService.ReinstateEmployeeAsync(id);
                return Ok(reinstatedEmployee);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
