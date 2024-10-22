using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Domain.Entities
{
    public class WorkflowStep
    {
        public int Id { get; set; }
    public int WorkflowDefinitionId { get; set; }
    public int Order { get; set; }
    public string ApproverRole { get; set; } = string.Empty;
        public WorkflowDefinition WorkflowDefinition { get; set; }
    public int EscalationTimeout { get; set; } // in hours
    public string EscalationApproverRole { get; set; } = string.Empty;
    }
}
