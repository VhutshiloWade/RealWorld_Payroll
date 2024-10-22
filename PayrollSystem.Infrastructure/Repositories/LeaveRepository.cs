using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveBalance>> GetLeaveBalancesAsync(DateTime asOfDate)
        {
            return await _context.LeaveBalances
                .Where(lb => lb.AsOfDate <= asOfDate)
                .GroupBy(lb => new { lb.EmployeeId, lb.LeaveType })
                .Select(g => new LeaveBalance
                {
                    EmployeeName = g.First().Employee.FirstName + " " + g.First().Employee.LastName,
                    LeaveType = g.Key.LeaveType,
                    TotalAllocated = g.Sum(lb => lb.AllocatedDays),
                    RemainingBalance = g.Sum(lb => lb.AllocatedDays) - g.Sum(lb => lb.UsedDays)
                })
                .ToListAsync();
        }
    }
}
