using System;

namespace PayrollSystem.Domain.Entities
{
    public class Shift
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ShiftType Type { get; set; }
        public Employee Employee { get; set; }
    }

    public enum ShiftType
    {
        Regular,
        Night,
        Weekend,
        Holiday
    }
}
