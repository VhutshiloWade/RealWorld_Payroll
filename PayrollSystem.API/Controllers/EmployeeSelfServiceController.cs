using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace PayrollSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeSelfServiceController : ControllerBase
    {
        private readonly IEmployeeSelfServiceService _selfServiceService;

        public EmployeeSelfServiceController(IEmployeeSelfServiceService selfServiceService)
        {
            _selfServiceService = selfServiceService;
        }

        [HttpGet("paystubs")]
        public async Task<IActionResult> GetPayStubs([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var payStubs = await _selfServiceService.GetPayStubsAsync(employeeId, startDate, endDate);
            return Ok(payStubs);
        }

        [HttpPost("leave-request")]
        public async Task<IActionResult> RequestLeave([FromBody] LeaveRequestDto leaveRequest)
        {
            leaveRequest.EmployeeId = User.FindFirst("EmployeeId")?.Value;
            var result = await _selfServiceService.RequestLeaveAsync(leaveRequest);
            return CreatedAtAction(nameof(RequestLeave), new { id = result.Id }, result);
        }

        [HttpGet("leave-requests")]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var leaveRequests = await _selfServiceService.GetLeaveRequestsAsync(employeeId);
            return Ok(leaveRequests);
        }

        [HttpPut("personal-info")]
        public async Task<IActionResult> UpdatePersonalInfo([FromBody] UpdatePersonalInfoDto updateInfo)
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var result = await _selfServiceService.UpdatePersonalInfoAsync(employeeId, updateInfo);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("personal-info")]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var personalInfo = await _selfServiceService.GetPersonalInfoAsync(employeeId);
            if (personalInfo != null)
            {
                return Ok(personalInfo);
            }
            return NotFound();
        }

        [HttpGet("paystub/{year}/{month}")]
        public async Task<IActionResult> GetPayStubForMonth(int year, int month)
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var payStub = await _selfServiceService.GetPayStubForMonthAsync(employeeId, year, month);
            if (payStub != null)
            {
                return Ok(payStub);
            }
            return NotFound();
        }

        [HttpGet("available-pay-periods")]
        public async Task<IActionResult> GetAvailablePayPeriods([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var payPeriods = await _selfServiceService.GetAvailablePayPeriodsAsync(employeeId, page, pageSize);
            return Ok(payPeriods);
        }

        [HttpGet("paystub-pdf/{year}/{month}")]
        public async Task<IActionResult> GetPayStubPdf(int year, int month)
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var pdfBytes = await _selfServiceService.GetPayStubPdfAsync(employeeId, year, month);
            if (pdfBytes != null)
            {
                return File(pdfBytes, "application/pdf", $"PayStub_{year}_{month:D2}.pdf");
            }
            return NotFound();
        }

        [HttpGet("yearly-summary/{year}")]
        public async Task<IActionResult> GetYearlySummary(int year)
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var summary = await _selfServiceService.GetYearlySummaryAsync(employeeId, year);
            if (summary != null)
            {
                return Ok(summary);
            }
            return NotFound();
        }
    }
}
