namespace PayrollSystem.Application.DTOs
{
    public class EmployeeBenefitDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int BenefitPlanId { get; set; }
        public string BenefitPlanName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public DateTime? TerminationDate { get; set; }
    }
}
