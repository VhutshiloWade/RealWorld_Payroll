public interface IWorkflowRepository
{
    Task<WorkflowDefinition> GetWorkflowDefinitionByTypeAsync(string processType);
    Task<WorkflowInstance> CreateWorkflowInstanceAsync(WorkflowInstance instance);
    Task<WorkflowInstance> GetWorkflowInstanceAsync(int id);
    Task UpdateWorkflowInstanceAsync(WorkflowInstance instance);
    Task<List<WorkflowInstance>> GetPendingWorkflowsForApproverAsync(string approverRole);
}
