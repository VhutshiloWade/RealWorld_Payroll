namespace PayrollSystem.Domain.Entities
{
    public class EmployeeBenefit
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int BenefitPlanId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public Employee Employee { get; set; }
        public BenefitPlan BenefitPlan { get; set; }
    }
}
