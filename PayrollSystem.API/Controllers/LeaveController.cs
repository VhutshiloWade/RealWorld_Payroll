using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("request")]
        public async Task<ActionResult<LeaveDto>> RequestLeave(LeaveDto leaveRequest)
        {
            try
            {
                var result = await _leaveService.RequestLeaveAsync(leaveRequest);
                return CreatedAtAction(nameof(GetEmployeeLeaves), new { employeeId = result.EmployeeId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Manager,HRAdmin")]
        [HttpPut("{leaveId}/status")]
        public async Task<ActionResult<LeaveDto>> UpdateLeaveStatus(int leaveId, [FromBody] LeaveStatus status)
        {
            try
            {
                var result = await _leaveService.UpdateLeaveStatusAsync(leaveId, status);
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

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<LeaveDto>>> GetEmployeeLeaves(int employeeId)
        {
            try
            {
                var leaves = await _leaveService.GetEmployeeLeavesAsync(employeeId);
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("balance/{employeeId}/{year}")]
        public async Task<ActionResult<LeaveBalanceDto>> GetLeaveBalance(int employeeId, int year)
        {
            try
            {
                var balance = await _leaveService.GetLeaveBalanceAsync(employeeId, year);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost("initialize-balance/{employeeId}/{year}")]
        public async Task<IActionResult> InitializeLeaveBalances(int employeeId, int year)
        {
            try
            {
                await _leaveService.InitializeLeaveBalancesAsync(employeeId, year);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
