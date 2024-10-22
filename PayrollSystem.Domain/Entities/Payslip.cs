using PayrollSystem.Domain.Entities;
public class Payslip
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public required Employee Employee { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetSalary { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}
