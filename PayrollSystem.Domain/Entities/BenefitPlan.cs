namespace PayrollSystem.Domain.Entities
{
    public class BenefitPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public BenefitType Type { get; set; }
    }

    public enum BenefitType
    {
        HealthInsurance,
        DentalInsurance,
        VisionInsurance,
        LifeInsurance,
        RetirementPlan,
        PaidTimeOff,
        Other
    }
}
