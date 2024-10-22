namespace PayrollSystem.Application.DTOs
{
    public class MedicalAidPlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MonthlyCost { get; set; }
        public string CoverageDetails { get; set; } = string.Empty;
        // Add other relevant properties
    }
}
