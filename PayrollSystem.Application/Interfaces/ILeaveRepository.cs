using PayrollSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Interfaces
{
    public interface ILeaveRepository
    {
        Task<List<LeaveBalance>> GetLeaveBalancesAsync(DateTime asOfDate);
    }
}
