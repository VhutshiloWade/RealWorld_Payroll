namespace PayrollSystem.Domain.Entities
{
    public class Payroll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal MedicalAidDeduction { get; set; }
        public decimal NetSalary { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal Bonus { get; set; }
        public decimal RetirementContribution { get; set; }
        public decimal UnionDues { get; set; }
        
        public required Employee Employee { get; set; }
    }
}
