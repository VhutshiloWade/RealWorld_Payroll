public class YearlySummaryDto
{
    public int Year { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPay { get; set; }
    public List<MonthlySummaryDto>? MonthlySummaries { get; set; }
}

public class MonthlySummaryDto
{
    public int Month { get; set; }
    public decimal Earnings { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetPay { get; set; }
}
