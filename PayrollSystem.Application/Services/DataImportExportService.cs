using CsvHelper;
using CsvHelper.Configuration;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Services
{
    public class DataImportExportService : IDataImportExportService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayrollRepository _payrollRepository;

        public DataImportExportService(IEmployeeRepository employeeRepository, IPayrollRepository payrollRepository)
        {
            _employeeRepository = employeeRepository;
            _payrollRepository = payrollRepository;
        }

        public async Task<List<EmployeeImportExportDto>> ImportEmployeesAsync(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<EmployeeImportExportDto>().ToList();

            foreach (var record in records)
            {
                var employee = new Employee
                {
                    EmployeeId = record.EmployeeId,
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email,
                    Department = record.Department,
                    Salary = record.Salary,
                    HireDate = record.HireDate
                };

                await _employeeRepository.AddAsync(employee);
            }

            return records;
        }

        public async Task<List<PayrollImportExportDto>> ImportPayrollDataAsync(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<PayrollImportExportDto>().ToList();

            foreach (var record in records)
            {
                var payroll = new Payroll
                {
                    EmployeeId = record.EmployeeId,
                    PayPeriodStart = record.PayPeriodStart,
                    PayPeriodEnd = record.PayPeriodEnd,
                    GrossSalary = record.GrossSalary,
                    NetSalary = record.NetSalary,
                    TaxDeductions = record.TaxDeductions,
                    OtherDeductions = record.OtherDeductions
                };

                await _payrollRepository.AddAsync(payroll);
            }

            return records;
        }

        public async Task<byte[]> ExportEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var records = employees.Select(e => new EmployeeImportExportDto
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Department = e.Department,
                Salary = e.Salary,
                HireDate = e.HireDate
            }).ToList();

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            
            await csv.WriteRecordsAsync(records);
            await writer.FlushAsync();
            return memoryStream.ToArray();
        }

        public async Task<byte[]> ExportPayrollDataAsync(DateTime startDate, DateTime endDate)
        {
            var payrollData = await _payrollRepository.GetPayrollDataByDateRangeAsync(startDate, endDate);
            var records = payrollData.Select(p => new PayrollImportExportDto
            {
                EmployeeId = p.EmployeeId,
                PayPeriodStart = p.PayPeriodStart,
                PayPeriodEnd = p.PayPeriodEnd,
                GrossSalary = p.GrossSalary,
                NetSalary = p.NetSalary,
                TaxDeductions = p.TaxDeductions,
                OtherDeductions = p.OtherDeductions
            }).ToList();

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            
            await csv.WriteRecordsAsync(records);
            await writer.FlushAsync();
            return memoryStream.ToArray();
        }
    }
}
