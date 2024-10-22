namespace PayrollSystem.Application.DTOs
{
    public class TaxBracketDto
    {
        public int Id { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal UpperLimit { get; set; }
        public decimal TaxRate { get; set; }
        public int TaxYear { get; set; }
        // Add other relevant properties
    }
}
