using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Application.DTOs
{
    public class WorkflowStepDto
    {
       public int Id { get; set; }
    public int Order { get; set; }
    public string ApproverRole { get; set; } = string.Empty;
    }
}