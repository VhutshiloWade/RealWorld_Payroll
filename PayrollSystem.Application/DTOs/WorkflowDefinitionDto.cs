using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Application.DTOs
{
    public class WorkflowDefinitionDto
    {
        public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
        public string ProcessType { get; set; } = string.Empty;
        public List<WorkflowStepDto>? Steps { get; set; }
    }
}