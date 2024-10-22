using PayrollSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEmployeeSelfServiceService
{
    Task<List<PayStubDto>> GetPayStubsAsync(string employeeId, DateTime startDate, DateTime endDate);
    Task<LeaveRequestDto> RequestLeaveAsync(LeaveRequestDto leaveRequest);
    Task<List<LeaveRequestDto>> GetLeaveRequestsAsync(string employeeId);
    Task<bool> UpdatePersonalInfoAsync(string employeeId, UpdatePersonalInfoDto updateInfo);
    Task<EmployeeDto> GetPersonalInfoAsync(string employeeId);
    Task<PayStubDto> GetPayStubForMonthAsync(string employeeId, int year, int month);
    Task<List<PayPeriodDto>> GetAvailablePayPeriodsAsync(string employeeId);
    Task<PagedResult<PayPeriodDto>> GetAvailablePayPeriodsAsync(string employeeId, int page, int pageSize);
    Task<byte[]> GetPayStubPdfAsync(string employeeId, int year, int month);
    Task<YearlySummaryDto> GetYearlySummaryAsync(string employeeId, int year);
}
