using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Application.Services;
using PayrollSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Services
{
    public class WorkflowEscalationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WorkflowEscalationService> _logger;

        public WorkflowEscalationService(IServiceProvider serviceProvider, ILogger<WorkflowEscalationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessEscalations();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Check every hour
            }
        }

        private async Task ProcessEscalations()
        {
            using var scope = _serviceProvider.CreateScope();
            var workflowRepository = scope.ServiceProvider.GetRequiredService<IWorkflowRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<WorkflowNotificationService>();

            var pendingWorkflows = await workflowRepository.GetAllPendingWorkflowsAsync();

            foreach (var workflow in pendingWorkflows)
            {
                var currentStep = workflow.WorkflowDefinition.Steps.FirstOrDefault(s => s.Order == workflow.CurrentStepOrder);
                if (currentStep == null) continue;

                var lastAction = workflow.Actions.OrderByDescending(a => a.ActionDate).FirstOrDefault();
                if (lastAction == null) continue;

                var timeSinceLastAction = DateTime.UtcNow - lastAction.ActionDate;
                if (timeSinceLastAction.TotalHours > currentStep.EscalationTimeout)
                {
                    workflow.CurrentStepOrder++;
                    await workflowRepository.UpdateWorkflowInstanceAsync(workflow);

                    await notificationService.SendPendingApprovalNotificationAsync(workflow, currentStep.EscalationApproverRole);

                    _logger.LogInformation($"Workflow {workflow.Id} escalated to {currentStep.EscalationApproverRole}");
                }
            }
        }
    }
}
