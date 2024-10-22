namespace PayrollSystem.Application.DTOs
{
    public class EmployeePersonalInfoDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public int? TaxBracketId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BankAccountNumber { get; set; } = string.Empty;
        public string TaxReferenceNumber { get; set; } = string.Empty;
        
    }
}
