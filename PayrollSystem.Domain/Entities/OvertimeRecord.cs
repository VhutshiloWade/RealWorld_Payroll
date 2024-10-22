using System;

namespace PayrollSystem.Domain.Entities
{
    public class OvertimeRecord
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public OvertimeType Type { get; set; }
        public bool IsApproved { get; set; }
        public Employee Employee { get; set; }
    }

    public enum OvertimeType
    {
        Regular,
        Weekend,
        Holiday
    }
}
