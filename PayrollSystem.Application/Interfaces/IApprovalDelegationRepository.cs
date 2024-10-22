public interface IApprovalDelegationRepository
{
    Task<ApprovalDelegation> CreateDelegationAsync(ApprovalDelegation delegation);
    Task<ApprovalDelegation> GetActiveDelegationAsync(string delegatorId);
    Task<List<ApprovalDelegation>> GetActiveDelegationsForDelegateAsync(string delegateId);
    Task UpdateDelegationAsync(ApprovalDelegation delegation);
}
