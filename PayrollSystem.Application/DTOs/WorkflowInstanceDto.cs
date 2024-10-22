using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Application.DTOs
{
    public class WorkflowInstanceDto
    {
        public int Id { get; set; }
    public int WorkflowDefinitionId { get; set; }
    public string Status { get; set; } = string.Empty;
        public int CurrentStepOrder { get; set; }
    public int RequestId { get; set; }
    public string RequestType { get; set; } = string.Empty;
        public List<WorkflowActionDto>? Actions { get; set; }
    }
}