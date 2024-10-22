using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;

namespace PayrollSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BenefitsController : ControllerBase
    {
        private readonly IBenefitsService _benefitsService;

        public BenefitsController(IBenefitsService benefitsService)
        {
            _benefitsService = benefitsService;
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPost("plans")]
        public async Task<ActionResult<BenefitPlanDto>> CreateBenefitPlan(BenefitPlanDto benefitPlanDto)
        {
            var result = await _benefitsService.CreateBenefitPlanAsync(benefitPlanDto);
            return CreatedAtAction(nameof(GetBenefitPlan), new { id = result.Id }, result);
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPut("plans/{id}")]
        public async Task<ActionResult<BenefitPlanDto>> UpdateBenefitPlan(int id, BenefitPlanDto benefitPlanDto)
        {
            try
            {
                var result = await _benefitsService.UpdateBenefitPlanAsync(id, benefitPlanDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpDelete("plans/{id}")]
        public async Task<IActionResult> DeleteBenefitPlan(int id)
        {
            await _benefitsService.DeleteBenefitPlanAsync(id);
            return NoContent();
        }

        [HttpGet("plans/{id}")]
        public async Task<ActionResult<BenefitPlanDto>> GetBenefitPlan(int id)
        {
            try
            {
                var result = await _benefitsService.GetBenefitPlanByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("plans")]
        public async Task<ActionResult<IEnumerable<BenefitPlanDto>>> GetAllBenefitPlans()
        {
            var result = await _benefitsService.GetAllBenefitPlansAsync();
            return Ok(result);
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPost("enroll")]
        public async Task<ActionResult<EmployeeBenefitDto>> EnrollEmployeeInBenefitPlan(int employeeId, int benefitPlanId)
        {
            try
            {
                var result = await _benefitsService.EnrollEmployeeInBenefitPlanAsync(employeeId, benefitPlanId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "HRAdmin,SystemAdmin")]
        [HttpPost("terminate/{employeeBenefitId}")]
        public async Task<ActionResult<EmployeeBenefitDto>> TerminateEmployeeBenefit(int employeeBenefitId)
        {
            try
            {
                var result = await _benefitsService.TerminateEmployeeBenefitAsync(employeeBenefitId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<EmployeeBenefitDto>>> GetEmployeeBenefits(int employeeId)
        {
            var result = await _benefitsService.GetEmployeeBenefitsAsync(employeeId);
            return Ok(result);
        }

        [HttpGet("employee/{employeeId}/total-cost")]
        public async Task<ActionResult<decimal>> GetEmployeeBenefitsTotalCost(int employeeId)
        {
            var result = await _benefitsService.CalculateTotalBenefitsCostAsync(employeeId);
            return Ok(result);
        }
    }
}
