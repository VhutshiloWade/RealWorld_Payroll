using PayrollSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Interfaces
{
    public interface IPayrollRepository
    {
        Task<Payroll> GetByIdAsync(int id);
        Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Payroll>> GetByPayPeriodAsync(DateTime start, DateTime end);
        Task AddAsync(Payroll payroll);
        Task<PayStub> GetPayStubForMonthAsync(string employeeId, int year, int month);
        Task<PagedResult<PayPeriod>> GetAvailablePayPeriodsAsync(string employeeId, int page, int pageSize);
        Task<List<PayStub>> GetPayStubsForYearAsync(string employeeId, int year);
        Task<Payslip> GetPayslipAsync(string employeeId, int year, int month);
        Task<List<MonthlyPayrollData>> GetMonthlyPayrollDataAsync(int year, int month);
        Task<List<AnnualPayrollData>> GetAnnualPayrollDataAsync(int year);

        Task<IEnumerable<Payroll>> GetByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}
