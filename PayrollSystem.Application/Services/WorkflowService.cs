using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Application.DTOs;

public class WorkflowService : IWorkflowService
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly ILogger<WorkflowService> _logger;
    private readonly WorkflowNotificationService _notificationService;
    private readonly ApprovalDelegationService _delegationService;

    public WorkflowService(
        IWorkflowRepository workflowRepository,
        WorkflowNotificationService notificationService,
        ILogger<WorkflowService> logger,
        ApprovalDelegationService delegationService)
    {
        _workflowRepository = workflowRepository;
        _logger = logger;
        _notificationService = notificationService;
        _delegationService = delegationService;
    }

    public async Task<WorkflowInstanceDto> InitiateWorkflowAsync(string processType, int requestId)
    {
        var workflowDefinition = await _workflowRepository.GetWorkflowDefinitionByTypeAsync(processType);
        if (workflowDefinition == null)
        {
            throw new InvalidOperationException($"No workflow defined for process type: {processType}");
        }

        var workflowInstance = new WorkflowInstance
        {
            WorkflowDefinitionId = workflowDefinition.Id,
            Status = "InProgress",
            CurrentStepOrder = 1,
            RequestId = requestId,
            RequestType = processType
        };

        var createdInstance = await _workflowRepository.CreateWorkflowInstanceAsync(workflowInstance);
        
        // Send notification for the first step
        await _notificationService.SendPendingApprovalNotificationAsync(createdInstance, createdInstance.WorkflowDefinition.Steps.First().ApproverRole);

        return MapToDto(createdInstance);
    }

    public async Task<WorkflowInstanceDto> ProcessWorkflowStepAsync(int workflowInstanceId, string action, string actorId, string comments)
    {
        var workflowInstance = await _workflowRepository.GetWorkflowInstanceAsync(workflowInstanceId);
        if (workflowInstance == null)
        {
            throw new KeyNotFoundException($"Workflow instance not found: {workflowInstanceId}");
        }

        var workflowAction = new WorkflowAction
        {
            WorkflowInstanceId = workflowInstanceId,
            StepOrder = workflowInstance.CurrentStepOrder,
            Action = action,
            ActorId = actorId,
            ActionDate = DateTime.UtcNow,
            Comments = comments
        };

        workflowInstance.Actions.Add(workflowAction);

        if (action == "Approve" && workflowInstance.CurrentStepOrder < workflowInstance.WorkflowDefinition.Steps.Count)
        {
            workflowInstance.CurrentStepOrder++;
            var nextStep = workflowInstance.WorkflowDefinition.Steps.FirstOrDefault(s => s.Order == workflowInstance.CurrentStepOrder);
            if (nextStep != null)
            {
                await _notificationService.SendPendingApprovalNotificationAsync(workflowInstance, nextStep.ApproverRole);
            }
        }
        else if (action == "Reject")
        {
            workflowInstance.Status = "Rejected";
        }

        await _workflowRepository.UpdateWorkflowInstanceAsync(workflowInstance);
        return MapToDto(workflowInstance);
    }

    public async Task<List<WorkflowInstanceDto>> GetPendingWorkflowsForApproverAsync(string approverRole)
    {
        var pendingWorkflows = await _workflowRepository.GetPendingWorkflowsForApproverAsync(approverRole);
        var effectiveApproverId = await _delegationService.GetEffectiveApproverId(approverRole);
        
        if (effectiveApproverId != approverRole)
        {
            var delegatedWorkflows = await _workflowRepository.GetPendingWorkflowsForApproverAsync(effectiveApproverId);
            pendingWorkflows.AddRange(delegatedWorkflows);
        }

        return pendingWorkflows.Select(MapToDto).ToList();
    }

    private WorkflowInstanceDto MapToDto(WorkflowInstance instance)
    {
        // Implement mapping logic
    }
}
