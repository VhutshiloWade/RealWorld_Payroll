using System;

namespace PayrollSystem.Domain.Entities
{
    public class PayPeriod
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        // Add other relevant properties as needed
    }
}
