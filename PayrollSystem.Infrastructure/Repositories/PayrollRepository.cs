using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Infrastructure.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly ApplicationDbContext _context;

        public PayrollRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payroll> GetByIdAsync(int id)
        {
            return await _context.Payrolls.FindAsync(id);
        }

        public async Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Payrolls
                .Where(p => p.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payroll>> GetByPayPeriodAsync(DateTime start, DateTime end)
        {
            return await _context.Payrolls
                .Where(p => p.PayPeriodStart >= start && p.PayPeriodEnd <= end)
                .ToListAsync();
        }

        public async Task AddAsync(Payroll payroll)
        {
            await _context.Payrolls.AddAsync(payroll);
            await _context.SaveChangesAsync();
        }

        public async Task<PayStub> GetPayStubForMonthAsync(string employeeId, int year, int month)
        {
            return await _context.PayStubs
                .Where(p => p.EmployeeId == employeeId &&
                            p.PayPeriodStart.Year == year &&
                            p.PayPeriodStart.Month == month)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<PayPeriod>> GetAvailablePayPeriodsAsync(string employeeId, int page, int pageSize)
        {
            var query = _context.PayStubs
                .Where(p => p.EmployeeId == employeeId)
                .Select(p => new PayPeriod
                {
                    Year = p.PayPeriodStart.Year,
                    Month = p.PayPeriodStart.Month,
                    PayDate = p.PayDate
                })
                .Distinct()
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<PayPeriod>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<List<PayStub>> GetPayStubsForYearAsync(string employeeId, int year)
        {
            return await _context.PayStubs
                .Where(p => p.EmployeeId == employeeId && p.PayPeriodStart.Year == year)
                .OrderBy(p => p.PayPeriodStart)
                .ToListAsync();
        }

        public async Task<Payslip> GetPayslipAsync(string employeeId, int year, int month)
        {
            return await _context.Payslips
                .Where(p => p.EmployeeId == employeeId && p.Year == year && p.Month == month)
                .FirstOrDefaultAsync();
        }

        public async Task<List<MonthlyPayrollData>> GetMonthlyPayrollDataAsync(int year, int month)
        {
            return await _context.Payslips
                .Where(p => p.Year == year && p.Month == month)
                .Select(p => new MonthlyPayrollData
                {
                    EmployeeName = p.Employee.FirstName + " " + p.Employee.LastName,
                    BasicSalary = p.BasicSalary,
                    Allowances = p.Allowances,
                    Deductions = p.Deductions,
                    NetSalary = p.NetSalary,
                    PaymentStatus = p.PaymentStatus
                })
                .ToListAsync();
        }

        public async Task<List<AnnualPayrollData>> GetAnnualPayrollDataAsync(int year)
        {
            return await _context.Payslips
                .Where(p => p.Year == year)
                .GroupBy(p => p.EmployeeId)
                .Select(g => new AnnualPayrollData
                {
                    EmployeeName = g.First().Employee.FirstName + " " + g.First().Employee.LastName,
                    TotalBasicSalary = g.Sum(p => p.BasicSalary),
                    TotalAllowances = g.Sum(p => p.Allowances),
                    TotalDeductions = g.Sum(p => p.Deductions),
                    TotalNetSalary = g.Sum(p => p.NetSalary)
                })
                .ToListAsync();
        }
    }
}
