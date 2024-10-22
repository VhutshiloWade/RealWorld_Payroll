namespace PayrollSystem.Application.DTOs
{
    public class PayrollCalculationRequestDto
    {
        public int EmployeeId { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal BonusPercentage { get; set; }
    }
}
