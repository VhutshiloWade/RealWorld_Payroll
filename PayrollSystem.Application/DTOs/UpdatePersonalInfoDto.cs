public class UpdatePersonalInfoDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string EmergencyContact { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;
    public string BankAccountNumber { get; set; } = string.Empty;
    public string TaxReferenceNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    // Add other fields that employees should be able to update
}
