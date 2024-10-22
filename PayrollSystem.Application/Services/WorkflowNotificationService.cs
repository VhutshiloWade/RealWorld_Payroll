using System;
using System.Threading.Tasks;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Models;

public class WorkflowNotificationService
{
    private readonly INotificationService _notificationService;
    private readonly IEmployeeRepository _employeeRepository;

    public WorkflowNotificationService(INotificationService notificationService, IEmployeeRepository employeeRepository)
    {
        _notificationService = notificationService;
        _employeeRepository = employeeRepository;
    }

    public async Task SendPendingApprovalNotificationAsync(WorkflowInstance workflow, string approverRole)
    {
        var approvers = await _employeeRepository.GetEmployeesByRoleAsync(approverRole);
        foreach (var approver in approvers)
        {
            var subject = $"Pending Approval: {workflow.RequestType} #{workflow.RequestId}";
            var body = $"You have a pending approval for {workflow.RequestType} #{workflow.RequestId}. Please review and take action.";
            await _notificationService.SendWorkflowNotificationAsync(approver.Email, subject, body);
        }
    }
}
