using PayrollSystem.Application.DTOs;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Interfaces
{
    public interface ILeaveService
    {
        Task<LeaveDto> RequestLeaveAsync(LeaveDto leaveRequest);
        Task<LeaveDto> UpdateLeaveStatusAsync(int leaveId, LeaveStatus status);
        Task<IEnumerable<LeaveDto>> GetEmployeeLeavesAsync(int employeeId);
        Task<decimal> CalculateLeaveAdjustmentAsync(int employeeId, DateTime payPeriodStart, DateTime payPeriodEnd);
        Task<LeaveBalanceDto> GetLeaveBalanceAsync(int employeeId, int year);
        Task UpdateLeaveBalanceAsync(int employeeId, LeaveType leaveType, decimal adjustment, int year);
        Task InitializeLeaveBalancesAsync(int employeeId, int year);
    }
}
