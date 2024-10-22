using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Domain.Entities
{
    public class WorkflowAction
    {
        public int Id { get; set; }
    public int WorkflowInstanceId { get; set; }
    public int StepOrder { get; set; }
    public string Action { get; set; } = string.Empty; // e.g., "Approve", "Reject"
        public string ActorId { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
    public string Comments { get; set; } = string.Empty;
        public WorkflowInstance WorkflowInstance { get; set; }
    }
}