using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Interfaces
{
    public interface IShiftOvertimeService
    {
        Task<ShiftDto> CreateShiftAsync(ShiftDto shiftDto);
        Task<ShiftDto> UpdateShiftAsync(int id, ShiftDto shiftDto);
        Task DeleteShiftAsync(int id);
        Task<IEnumerable<ShiftDto>> GetEmployeeShiftsAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<OvertimeRecordDto> CreateOvertimeRecordAsync(OvertimeRecordDto overtimeRecordDto);
        Task<OvertimeRecordDto> ApproveOvertimeRecordAsync(int id);
        Task<IEnumerable<OvertimeRecordDto>> GetEmployeeOvertimeRecordsAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<decimal> CalculateOvertimeHoursAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<decimal> CalculateShiftDifferentialAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}
