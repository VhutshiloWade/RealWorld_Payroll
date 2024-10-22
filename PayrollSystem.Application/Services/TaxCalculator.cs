using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Services
{
    public class TaxCalculator
    {
        public decimal CalculatePAYE(decimal taxableIncome, IEnumerable<TaxBracket> taxBrackets, decimal taxCredits)
        {
            decimal totalTax = 0;
            decimal remainingIncome = taxableIncome;

            foreach (var bracket in taxBrackets.OrderBy(b => b.LowerLimit))
            {
                if (remainingIncome <= 0) break;

                decimal taxableAmountInBracket;
                if (bracket.UpperLimit.HasValue)
                {
                    taxableAmountInBracket = Math.Min(remainingIncome, bracket.UpperLimit.Value - bracket.LowerLimit);
                }
                else
                {
                    taxableAmountInBracket = remainingIncome;
                }

                totalTax += taxableAmountInBracket * bracket.Rate;
                remainingIncome -= taxableAmountInBracket;
            }

            return Math.Max(totalTax - taxCredits, 0);
        }

        public decimal CalculateAnnualTaxReconciliation(decimal totalAnnualIncome, decimal totalTaxPaid, IEnumerable<TaxBracket> taxBrackets, decimal annualTaxCredits)
        {
            decimal calculatedAnnualTax = CalculatePAYE(totalAnnualIncome, taxBrackets, annualTaxCredits);
            return calculatedAnnualTax - totalTaxPaid;
        }
    }
}
