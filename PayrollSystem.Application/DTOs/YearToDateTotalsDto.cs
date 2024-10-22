namespace PayrollSystem.Application.DTOs
{
    public class YearToDateTotalsDto
    {
        public decimal GrossSalary { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal Bonus { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal MedicalAidDeduction { get; set; }
        public decimal RetirementContribution { get; set; }
        public decimal UnionDues { get; set; }
        public decimal NetSalary { get; set; }
    }
}
