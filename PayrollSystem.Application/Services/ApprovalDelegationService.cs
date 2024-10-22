using System;
using System.Threading.Tasks;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;

public class ApprovalDelegationService
{
    private readonly IApprovalDelegationRepository _delegationRepository;

    public ApprovalDelegationService(IApprovalDelegationRepository delegationRepository)
    {
        _delegationRepository = delegationRepository;
    }

    public async Task<ApprovalDelegation> CreateDelegationAsync(string delegatorId, string delegateId, DateTime startDate, DateTime endDate)
    {
        var delegation = new ApprovalDelegation
        {
            DelegatorId = delegatorId,
            DelegateId = delegateId,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true
        };

        return await _delegationRepository.CreateDelegationAsync(delegation);
    }

    public async Task<string> GetEffectiveApproverId(string approverId)
    {
        var activeDelegation = await _delegationRepository.GetActiveDelegationAsync(approverId);
        return activeDelegation != null ? activeDelegation.DelegateId : approverId;
    }
}
