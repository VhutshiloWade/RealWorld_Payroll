using PayrollSystem.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GeneratePayslipPdfAsync(string employeeId, int year, int month);
        Task<byte[]> GenerateMonthlyPayrollReportAsync(int year, int month);
        Task<byte[]> GenerateAnnualPayrollReportAsync(int year);
        Task<byte[]> GenerateLeaveBalanceReportAsync(DateTime asOfDate);
    }
}
