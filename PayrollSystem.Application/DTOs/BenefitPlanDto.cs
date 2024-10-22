using PayrollSystem.Domain.Entities;
namespace PayrollSystem.Application.DTOs
{
    public class BenefitPlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public BenefitType Type { get; set; }
    }
}
