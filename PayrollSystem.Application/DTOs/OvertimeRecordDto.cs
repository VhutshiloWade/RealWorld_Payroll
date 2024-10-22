using System;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.DTOs
{
    public class OvertimeRecordDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public OvertimeType Type { get; set; }
        public bool IsApproved { get; set; }
    }
}
