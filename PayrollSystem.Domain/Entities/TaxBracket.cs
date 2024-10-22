namespace PayrollSystem.Domain.Entities
{
    public class TaxBracket
    {
        public int Id { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal UpperLimit { get; set; }
        public decimal Rate { get; set; }
    }
}
