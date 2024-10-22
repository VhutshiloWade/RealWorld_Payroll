using iTextSharp.text;
using iTextSharp.text.pdf;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Application.DTOs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRepository _leaveRepository;

        public ReportService(IPayrollRepository payrollRepository, IEmployeeRepository employeeRepository, ILeaveRepository leaveRepository)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
            _leaveRepository = leaveRepository;
        }

        public async Task<byte[]> GeneratePayslipPdfAsync(string employeeId, int year, int month)
        {
            var payslip = await _payrollRepository.GetPayslipAsync(employeeId, year, month);
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();
                document.Add(new Paragraph($"Payslip for {employee.FirstName} {employee.LastName}"));
                document.Add(new Paragraph($"Period: {year}-{month:D2}"));
                document.Add(new Paragraph($"Basic Salary: ${payslip.BasicSalary:F2}"));
                document.Add(new Paragraph($"Allowances: ${payslip.Allowances:F2}"));
                document.Add(new Paragraph($"Deductions: ${payslip.Deductions:F2}"));
                document.Add(new Paragraph($"Net Salary: ${payslip.NetSalary:F2}"));
                document.Close();

                return ms.ToArray();
            }
        }

        public async Task<byte[]> GenerateMonthlyPayrollReportAsync(int year, int month)
        {
            var payrollData = await _payrollRepository.GetMonthlyPayrollDataAsync(year, month);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4.Rotate(), 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();
                document.Add(new Paragraph($"Monthly Payroll Report for {year}-{month:D2}"));

                PdfPTable table = new PdfPTable(6);
                table.AddCell("Employee");
                table.AddCell("Basic Salary");
                table.AddCell("Allowances");
                table.AddCell("Deductions");
                table.AddCell("Net Salary");
                table.AddCell("Payment Status");

                foreach (var payroll in payrollData)
                {
                    table.AddCell($"{payroll.EmployeeName}");
                    table.AddCell($"${payroll.BasicSalary:F2}");
                    table.AddCell($"${payroll.Allowances:F2}");
                    table.AddCell($"${payroll.Deductions:F2}");
                    table.AddCell($"${payroll.NetSalary:F2}");
                    table.AddCell($"{payroll.PaymentStatus}");
                }

                document.Add(table);
                document.Close();

                return ms.ToArray();
            }
        }

        public async Task<byte[]> GenerateAnnualPayrollReportAsync(int year)
        {
            var annualPayrollData = await _payrollRepository.GetAnnualPayrollDataAsync(year);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4.Rotate(), 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();
                document.Add(new Paragraph($"Annual Payroll Report for {year}"));

                PdfPTable table = new PdfPTable(5);
                table.AddCell("Employee");
                table.AddCell("Total Basic Salary");
                table.AddCell("Total Allowances");
                table.AddCell("Total Deductions");
                table.AddCell("Total Net Salary");

                foreach (var payroll in annualPayrollData)
                {
                    table.AddCell($"{payroll.EmployeeName}");
                    table.AddCell($"${payroll.TotalBasicSalary:F2}");
                    table.AddCell($"${payroll.TotalAllowances:F2}");
                    table.AddCell($"${payroll.TotalDeductions:F2}");
                    table.AddCell($"${payroll.TotalNetSalary:F2}");
                }

                document.Add(table);
                document.Close();

                return ms.ToArray();
            }
        }

        public async Task<byte[]> GenerateLeaveBalanceReportAsync(DateTime asOfDate)
        {
            var leaveBalances = await _leaveRepository.GetLeaveBalancesAsync(asOfDate);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();
                document.Add(new Paragraph($"Leave Balance Report as of {asOfDate:d}"));

                PdfPTable table = new PdfPTable(4);
                table.AddCell("Employee");
                table.AddCell("Leave Type");
                table.AddCell("Total Allocated");
                table.AddCell("Remaining Balance");

                foreach (var balance in leaveBalances)
                {
                    table.AddCell($"{balance.EmployeeName}");
                    table.AddCell($"{balance.LeaveType}");
                    table.AddCell($"{balance.TotalAllocated}");
                    table.AddCell($"{balance.RemainingBalance}");
                }

                document.Add(table);
                document.Close();

                return ms.ToArray();
            }
        }
    }
}
