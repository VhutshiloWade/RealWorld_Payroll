using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace PayrollSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataImportExportController : ControllerBase
    {
        private readonly IDataImportExportService _dataImportExportService;

        public DataImportExportController(IDataImportExportService dataImportExportService)
        {
            _dataImportExportService = dataImportExportService;
        }

        [HttpPost("import/employees")]
        public async Task<IActionResult> ImportEmployees(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            using var stream = file.OpenReadStream();
            var importedEmployees = await _dataImportExportService.ImportEmployeesAsync(stream);
            return Ok($"Successfully imported {importedEmployees.Count} employees");
        }

        [HttpPost("import/payroll")]
        public async Task<IActionResult> ImportPayrollData(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            using var stream = file.OpenReadStream();
            var importedPayrollData = await _dataImportExportService.ImportPayrollDataAsync(stream);
            return Ok($"Successfully imported {importedPayrollData.Count} payroll records");
        }

        [HttpGet("export/employees")]
        public async Task<IActionResult> ExportEmployees()
        {
            var fileContents = await _dataImportExportService.ExportEmployeesAsync();
            return File(fileContents, "text/csv", "employees_export.csv");
        }

        [HttpGet("export/payroll")]
        public async Task<IActionResult> ExportPayrollData([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var fileContents = await _dataImportExportService.ExportPayrollDataAsync(startDate, endDate);
            return File(fileContents, "text/csv", "payroll_export.csv");
        }
    }
}
