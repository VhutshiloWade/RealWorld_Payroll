using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Application.DTOs
{
    public class WorkflowActionDto
    {
        public int Id { get; set; }
    public int StepOrder { get; set; }
    public string Action { get; set; } = string.Empty;
        public string ActorId { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
    public string Comments { get; set; } = string.Empty;
    }
}