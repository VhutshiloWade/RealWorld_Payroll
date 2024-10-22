using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Interfaces
{
    public interface IPayrollService
    {
        Task<PayrollDto> CalculatePayrollAsync(int employeeId, DateTime payPeriodStart, DateTime payPeriodEnd, decimal overtimeHours = 0, decimal bonusPercentage = 0);
        Task ProcessPayrollForAllEmployeesAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<PayrollDto>> GetPayrollHistoryForEmployeeAsync(int employeeId);
        Task<IEnumerable<PayrollDto>> GetPayrollsByPeriodAsync(DateTime payPeriodStart, DateTime payPeriodEnd);
        Task<YearToDateTotalsDto> GetYearToDateTotalsAsync(int employeeId, int year);
        Task<decimal> CalculateAnnualTaxReconciliationAsync(int employeeId, int year);
    }
}
