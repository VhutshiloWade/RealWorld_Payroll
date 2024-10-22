
using PayrollSystem.Application.DTOs;

public interface IWorkflowService
{
    Task<WorkflowInstanceDto> InitiateWorkflowAsync(string processType, int requestId);
    Task<WorkflowInstanceDto> ProcessWorkflowStepAsync(int workflowInstanceId, string action, string actorId, string comments);
    Task<List<WorkflowInstanceDto>> GetPendingWorkflowsForApproverAsync(string approverRole);
}
