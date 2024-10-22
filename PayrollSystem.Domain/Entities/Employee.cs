namespace PayrollSystem.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public decimal BaseSalary { get; set; }
        public int TaxBracketId { get; set; }
        public int MedicalAidPlanId { get; set; }
        public int DepartmentId { get; set; }
        
        public TaxBracket? TaxBracket { get; set; }
        public MedicalAidPlan? MedicalAidPlan { get; set; }
        public Department? Department { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public DateTime? TerminationDate { get; set; }
    }
}
