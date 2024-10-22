public class PayStubDto
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public decimal TaxDeductions { get; set; }
    public decimal OtherDeductions { get; set; }
    // Add other relevant fields
}
