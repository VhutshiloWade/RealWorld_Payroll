using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Domain.Entities
{
    public class WorkflowInstance
    {
       public int Id { get; set; }
    public int WorkflowDefinitionId { get; set; }
    public string Status { get; set; } = string.Empty; // e.g., "InProgress", "Approved", "Rejected"
        public int CurrentStepOrder { get; set; }
    public int RequestId { get; set; } // ID of the leave request or overtime record
    public string RequestType { get; set; } = string.Empty; // e.g., "LeaveRequest", "OvertimeApproval"
        public WorkflowDefinition WorkflowDefinition { get; set; }
    public List<WorkflowAction> Actions { get; set; } = new List<WorkflowAction>(); 
    }
}