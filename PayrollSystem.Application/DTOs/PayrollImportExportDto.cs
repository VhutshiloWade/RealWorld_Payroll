public class PayrollImportExportDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal NetSalary { get; set; }
    public decimal TaxDeductions { get; set; }
    public decimal OtherDeductions { get; set; }
}
