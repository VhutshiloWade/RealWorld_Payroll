using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Domain.Entities
{
    public class WorkflowDefinition
    {
        public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
        public string ProcessType { get; set; } = string.Empty; // e.g., "LeaveRequest", "OvertimeApproval"
        public List<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
    }
}