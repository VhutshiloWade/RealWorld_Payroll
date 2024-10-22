using System;

namespace PayrollSystem.Application.DTOs
{
    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public decimal? Salary { get; set; }
        public decimal BaseSalary { get; set; }
        public int TaxBracketId { get; set; }
        public int MedicalAidPlanId { get; set; }
        public bool IsActive { get; set; }
        
        // Add other properties that can be updated
        // Note: Properties are nullable to allow partial updates
    }
}
