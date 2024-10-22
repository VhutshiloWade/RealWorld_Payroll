

using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.DTOs
{
    public class LeaveDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public LeaveStatus Status { get; set; }
        public string? Comments { get; set; }
    }
}
