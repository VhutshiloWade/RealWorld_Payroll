using PayrollSystem.Application.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public interface IDataImportExportService
{
    Task<List<EmployeeImportExportDto>> ImportEmployeesAsync(Stream fileStream);
    Task<List<PayrollImportExportDto>> ImportPayrollDataAsync(Stream fileStream);
    Task<byte[]> ExportEmployeesAsync();
    Task<byte[]> ExportPayrollDataAsync(DateTime startDate, DateTime endDate);
}
