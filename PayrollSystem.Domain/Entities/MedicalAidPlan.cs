namespace PayrollSystem.Domain.Entities
{
    public class MedicalAidPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MonthlyPremium { get; set; }
    }
}
