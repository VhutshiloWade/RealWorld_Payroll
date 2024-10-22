using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class EmployeeSelfServiceService : IEmployeeSelfServiceService
{
    private readonly IPayrollRepository _payrollRepository;
    private readonly ILeaveRepository _leaveRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeSelfServiceService(
        IPayrollRepository payrollRepository,
        ILeaveRepository leaveRepository,
        IEmployeeRepository employeeRepository)
    {
        _payrollRepository = payrollRepository;
        _leaveRepository = leaveRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<List<PayStubDto>> GetPayStubsAsync(string employeeId, DateTime startDate, DateTime endDate)
    {
        var payStubs = await _payrollRepository.GetPayStubsAsync(employeeId, startDate, endDate);
        return payStubs.Select(p => new PayStubDto
        {
            Id = p.Id,
            EmployeeId = p.EmployeeId,
            PayPeriodStart = p.PayPeriodStart,
            PayPeriodEnd = p.PayPeriodEnd,
            GrossPay = p.GrossPay,
            NetPay = p.NetPay,
            TaxDeductions = p.TaxDeductions,
            OtherDeductions = p.OtherDeductions
        }).ToList();
    }

    public async Task<LeaveRequestDto> RequestLeaveAsync(LeaveRequestDto leaveRequest)
    {
        var leave = new Leave
        {
            EmployeeId = leaveRequest.EmployeeId,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            LeaveType = leaveRequest.LeaveType,
            Status = "Pending",
            Reason = leaveRequest.Reason
        };

        var createdLeave = await _leaveRepository.CreateLeaveRequestAsync(leave);
        return new LeaveRequestDto
        {
            Id = createdLeave.Id,
            EmployeeId = createdLeave.EmployeeId,
            StartDate = createdLeave.StartDate,
            EndDate = createdLeave.EndDate,
            LeaveType = createdLeave.LeaveType,
            Status = createdLeave.Status,
            Reason = createdLeave.Reason
        };
    }

    public async Task<List<LeaveRequestDto>> GetLeaveRequestsAsync(string employeeId)
    {
        var leaveRequests = await _leaveRepository.GetLeaveRequestsAsync(employeeId);
        return leaveRequests.Select(l => new LeaveRequestDto
        {
            Id = l.Id,
            EmployeeId = l.EmployeeId,
            StartDate = l.StartDate,
            EndDate = l.EndDate,
            LeaveType = l.LeaveType,
            Status = l.Status,
            Reason = l.Reason
        }).ToList();
    }

    public async Task<bool> UpdatePersonalInfoAsync(string employeeId, UpdatePersonalInfoDto updateInfo)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            return false;
        }

        employee.PhoneNumber = updateInfo.PhoneNumber;
        employee.Address = updateInfo.Address;
        employee.EmergencyContact = updateInfo.EmergencyContact;
        // Update other fields as necessary

        await _employeeRepository.UpdateAsync(employee);
        return true;
    }

    public async Task<EmployeeDto> GetPersonalInfoAsync(string employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            return null;
        }

        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Address = employee.Address,
            EmergencyContact = employee.EmergencyContact
            // Map other relevant fields
        };
    }

    public async Task<PayStubDto> GetPayStubForMonthAsync(string employeeId, int year, int month)
    {
        var payStub = await _payrollRepository.GetPayStubForMonthAsync(employeeId, year, month);
        if (payStub == null)
        {
            return null;
        }

        return new PayStubDto
        {
            Id = payStub.Id,
            EmployeeId = payStub.EmployeeId,
            PayPeriodStart = payStub.PayPeriodStart,
            PayPeriodEnd = payStub.PayPeriodEnd,
            GrossPay = payStub.GrossPay,
            NetPay = payStub.NetPay,
            TaxDeductions = payStub.TaxDeductions,
            OtherDeductions = payStub.OtherDeductions
            // Map other fields as necessary
        };
    }

    public async Task<PagedResult<PayPeriodDto>> GetAvailablePayPeriodsAsync(string employeeId, int page, int pageSize)
    {
        var result = await _payrollRepository.GetAvailablePayPeriodsAsync(employeeId, page, pageSize);
        return new PagedResult<PayPeriodDto>
        {
            Items = result.Items.Select(p => new PayPeriodDto
            {
                Year = p.Year,
                Month = p.Month,
                PayDate = p.PayDate
            }).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<byte[]> GetPayStubPdfAsync(string employeeId, int year, int month)
    {
        var payStub = await GetPayStubForMonthAsync(employeeId, year, month);
        if (payStub == null)
        {
            return null;
        }

        using (MemoryStream ms = new MemoryStream())
        {
            Document document = new Document(PageSize.A4, 50, 50, 25, 25);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            document.Open();
            document.Add(new Paragraph($"Pay Stub for {year}-{month:D2}"));
            document.Add(new Paragraph($"Employee ID: {payStub.EmployeeId}"));
            document.Add(new Paragraph($"Pay Period: {payStub.PayPeriodStart:d} to {payStub.PayPeriodEnd:d}"));
            document.Add(new Paragraph($"Gross Pay: ${payStub.GrossPay:F2}"));
            document.Add(new Paragraph($"Tax Deductions: ${payStub.TaxDeductions:F2}"));
            document.Add(new Paragraph($"Other Deductions: ${payStub.OtherDeductions:F2}"));
            document.Add(new Paragraph($"Net Pay: ${payStub.NetPay:F2}"));
            document.Close();

            return ms.ToArray();
        }
    }

    public async Task<YearlySummaryDto> GetYearlySummaryAsync(string employeeId, int year)
    {
        var payStubs = await _payrollRepository.GetPayStubsForYearAsync(employeeId, year);
        var monthlySummaries = payStubs
            .GroupBy(p => p.PayPeriodStart.Month)
            .Select(g => new MonthlySummaryDto
            {
                Month = g.Key,
                Earnings = g.Sum(p => p.GrossPay),
                Deductions = g.Sum(p => p.TaxDeductions + p.OtherDeductions),
                NetPay = g.Sum(p => p.NetPay)
            })
            .OrderBy(m => m.Month)
            .ToList();

        return new YearlySummaryDto
        {
            Year = year,
            TotalEarnings = monthlySummaries.Sum(m => m.Earnings),
            TotalDeductions = monthlySummaries.Sum(m => m.Deductions),
            NetPay = monthlySummaries.Sum(m => m.NetPay),
            MonthlySummaries = monthlySummaries
        };
    }
}
