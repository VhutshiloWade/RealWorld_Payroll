namespace PayrollSystem.Domain.Entities
{
    public class LeaveBalance
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public Employee Employee { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public DateTime AsOfDate { get; set; }
        public int AllocatedDays { get; set; }
        public int UsedDays { get; set; }
    }
}
