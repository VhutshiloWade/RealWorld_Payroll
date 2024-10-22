using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;

namespace PayrollSystem.API.Controllers
{

[ApiController]
[Route("api/[controller]")]
public class WorkflowController : ControllerBase
{
    private readonly IWorkflowService _workflowService;

    public WorkflowController(IWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost("process")]
    public async Task<ActionResult<WorkflowInstanceDto>> ProcessWorkflowStep(int workflowInstanceId, string action, string comments)
    {
        var actorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _workflowService.ProcessWorkflowStepAsync(workflowInstanceId, action, actorId, comments);
        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<List<WorkflowInstanceDto>>> GetPendingWorkflows()
    {
        var approverRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var result = await _workflowService.GetPendingWorkflowsForApproverAsync(approverRole);
        return Ok(result);
    }
    }
}
