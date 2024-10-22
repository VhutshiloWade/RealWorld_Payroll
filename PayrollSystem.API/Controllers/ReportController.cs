using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace PayrollSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("payslip/{employeeId}/{year}/{month}")]
        public async Task<IActionResult> GetPayslipPdf(string employeeId, int year, int month)
        {
            var pdfBytes = await _reportService.GeneratePayslipPdfAsync(employeeId, year, month);
            return File(pdfBytes, "application/pdf", $"Payslip_{employeeId}_{year}_{month}.pdf");
        }

        [HttpGet("monthly-payroll/{year}/{month}")]
        [Authorize(Roles = "HR,Manager")]
        public async Task<IActionResult> GetMonthlyPayrollReport(int year, int month)
        {
            var pdfBytes = await _reportService.GenerateMonthlyPayrollReportAsync(year, month);
            return File(pdfBytes, "application/pdf", $"MonthlyPayrollReport_{year}_{month}.pdf");
        }

        [HttpGet("annual-payroll/{year}")]
        [Authorize(Roles = "HR,Manager")]
        public async Task<IActionResult> GetAnnualPayrollReport(int year)
        {
            var pdfBytes = await _reportService.GenerateAnnualPayrollReportAsync(year);
            return File(pdfBytes, "application/pdf", $"AnnualPayrollReport_{year}.pdf");
        }

        [HttpGet("leave-balance")]
        [Authorize(Roles = "HR,Manager")]
        public async Task<IActionResult> GetLeaveBalanceReport([FromQuery] DateTime asOfDate)
        {
            var pdfBytes = await _reportService.GenerateLeaveBalanceReportAsync(asOfDate);
            return File(pdfBytes, "application/pdf", $"LeaveBalanceReport_{asOfDate:yyyyMMdd}.pdf");
        }
    }
}
