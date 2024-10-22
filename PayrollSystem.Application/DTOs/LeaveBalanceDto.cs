using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.DTOs
{
    public class LeaveBalanceDto
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public Dictionary<LeaveType, decimal>? Balances { get; set; }
    }
}
