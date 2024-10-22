using System;

namespace PayrollSystem.Application.DTOs
{
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public int DepartmentId { get; set; }
        public decimal InitialSalary { get; set; }
        public decimal BaseSalary { get; set; }
        public int TaxBracketId { get; set; }
        public int MedicalAidPlanId { get; set; }
        public bool IsActive { get; set; }
        
        // Add other relevant properties for creating an employee
    }
}
