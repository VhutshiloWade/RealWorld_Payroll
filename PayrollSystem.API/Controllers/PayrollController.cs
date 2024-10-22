using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;

namespace PayrollSystem.API.Controllers
{
    [Authorize(Roles = "HRAdmin,SystemAdmin")]
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<PayrollDto>> CalculatePayroll([FromBody] PayrollCalculationRequestDto request)
        {
            try
            {
                var result = await _payrollService.CalculatePayrollAsync(
                    request.EmployeeId,
                    request.PayPeriodStart,
                    request.PayPeriodEnd,
                    request.OvertimeHours,
                    request.BonusPercentage
                );
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("ytd-totals/{employeeId}/{year}")]
        public async Task<ActionResult<YearToDateTotalsDto>> GetYearToDateTotals(int employeeId, int year)
        {
            try
            {
                var result = await _payrollService.GetYearToDateTotalsAsync(employeeId, year);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("annual-tax-reconciliation/{employeeId}/{year}")]
        public async Task<ActionResult<decimal>> CalculateAnnualTaxReconciliation(int employeeId, int year)
        {
            try
            {
                var result = await _payrollService.CalculateAnnualTaxReconciliationAsync(employeeId, year);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayroll([FromBody] PayPeriodDto payPeriod)
        {
            try
            {
                await _payrollService.ProcessPayrollForAllEmployeesAsync(payPeriod.Start, payPeriod.End);
                return Ok("Payroll processed successfully for all employees.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the payroll.");
            }
        }
    }
}
