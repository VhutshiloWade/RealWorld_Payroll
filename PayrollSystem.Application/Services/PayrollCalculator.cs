using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Services
{
    public class PayrollCalculator
    {
        public decimal CalculateOvertimePay(decimal regularHourlyRate, decimal overtimeHours, decimal overtimeRate = 1.5m)
        {
            return regularHourlyRate * overtimeHours * overtimeRate;
        }

        public decimal CalculateBonus(decimal baseSalary, decimal bonusPercentage)
        {
            return baseSalary * (bonusPercentage / 100);
        }

        public decimal CalculateDeduction(decimal grossSalary, DeductionType deductionType, decimal deductionRate)
        {
            return deductionType switch
            {
                DeductionType.Percentage => grossSalary * (deductionRate / 100),
                DeductionType.FixedAmount => deductionRate,
                _ => throw new ArgumentException("Invalid deduction type")
            };
        }

        public decimal CalculateProRataSalary(decimal monthlySalary, DateTime startDate, DateTime endDate)
        {
            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var workedDays = (endDate - startDate).Days + 1;
            return monthlySalary * workedDays / daysInMonth;
        }
    }

    public enum DeductionType
    {
        Percentage,
        FixedAmount
    }
}
