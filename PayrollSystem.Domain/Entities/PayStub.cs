using System;

namespace PayrollSystem.Domain.Entities
{
    public class PayStub
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime PayDate { get; set; }
        public decimal GrossPay { get; set; }
        public decimal NetPay { get; set; }
        public decimal Deductions { get; set; }
        // Add other relevant properties as needed
    }
}
