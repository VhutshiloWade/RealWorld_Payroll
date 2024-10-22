public class ApprovalDelegation
{
    public int Id { get; set; }
    public string DelegatorId { get; set; } = string.Empty;
    public string DelegateId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}
