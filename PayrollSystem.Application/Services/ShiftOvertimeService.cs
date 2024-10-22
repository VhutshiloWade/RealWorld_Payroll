using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Services
{
    public class ShiftOvertimeService : IShiftOvertimeService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IOvertimeRecordRepository _overtimeRecordRepository;

        public ShiftOvertimeService(IShiftRepository shiftRepository, IOvertimeRecordRepository overtimeRecordRepository)
        {
            _shiftRepository = shiftRepository;
            _overtimeRecordRepository = overtimeRecordRepository;
        }

        public async Task<ShiftDto> CreateShiftAsync(ShiftDto shiftDto)
        {
            var shift = new Shift
            {
                EmployeeId = shiftDto.EmployeeId,
                StartTime = shiftDto.StartTime,
                EndTime = shiftDto.EndTime,
                Type = shiftDto.Type
            };

            await _shiftRepository.AddAsync(shift);
            return MapToShiftDto(shift);
        }

        public async Task<ShiftDto> UpdateShiftAsync(int id, ShiftDto shiftDto)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null)
                throw new KeyNotFoundException($"Shift with ID {id} not found.");

            shift.StartTime = shiftDto.StartTime;
            shift.EndTime = shiftDto.EndTime;
            shift.Type = shiftDto.Type;

            await _shiftRepository.UpdateAsync(shift);
            return MapToShiftDto(shift);
        }

        public async Task DeleteShiftAsync(int id)
        {
            await _shiftRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ShiftDto>> GetEmployeeShiftsAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var shifts = await _shiftRepository.GetByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate);
            return shifts.Select(MapToShiftDto);
        }

        public async Task<OvertimeRecordDto> CreateOvertimeRecordAsync(OvertimeRecordDto overtimeRecordDto)
        {
            var overtimeRecord = new OvertimeRecord
            {
                EmployeeId = overtimeRecordDto.EmployeeId,
                Date = overtimeRecordDto.Date,
                Hours = overtimeRecordDto.Hours,
                Type = overtimeRecordDto.Type,
                IsApproved = false
            };

            await _overtimeRecordRepository.AddAsync(overtimeRecord);
            return MapToOvertimeRecordDto(overtimeRecord);
        }

        public async Task<OvertimeRecordDto> ApproveOvertimeRecordAsync(int id)
        {
            var overtimeRecord = await _overtimeRecordRepository.GetByIdAsync(id);
            if (overtimeRecord == null)
                throw new KeyNotFoundException($"Overtime record with ID {id} not found.");

            overtimeRecord.IsApproved = true;
            await _overtimeRecordRepository.UpdateAsync(overtimeRecord);
            return MapToOvertimeRecordDto(overtimeRecord);
        }

        public async Task<IEnumerable<OvertimeRecordDto>> GetEmployeeOvertimeRecordsAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var overtimeRecords = await _overtimeRecordRepository.GetByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate);
            return overtimeRecords.Select(MapToOvertimeRecordDto);
        }

        public async Task<decimal> CalculateOvertimeHoursAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var overtimeRecords = await _overtimeRecordRepository.GetByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate);
            return overtimeRecords.Where(r => r.IsApproved).Sum(r => r.Hours);
        }

        public async Task<decimal> CalculateShiftDifferentialAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var shifts = await _shiftRepository.GetByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate);
            decimal differential = 0;

            foreach (var shift in shifts)
            {
                switch (shift.Type)
                {
                    case ShiftType.Night:
                        differential += 0.1m * (decimal)(shift.EndTime - shift.StartTime).TotalHours;
                        break;
                    case ShiftType.Weekend:
                        differential += 0.15m * (decimal)(shift.EndTime - shift.StartTime).TotalHours;
                        break;
                    case ShiftType.Holiday:
                        differential += 0.2m * (decimal)(shift.EndTime - shift.StartTime).TotalHours;
                        break;
                }
            }

            return differential;
        }

        private ShiftDto MapToShiftDto(Shift shift)
        {
            return new ShiftDto
            {
                Id = shift.Id,
                EmployeeId = shift.EmployeeId,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime,
                Type = shift.Type
            };
        }

        private OvertimeRecordDto MapToOvertimeRecordDto(OvertimeRecord overtimeRecord)
        {
            return new OvertimeRecordDto
            {
                Id = overtimeRecord.Id,
                EmployeeId = overtimeRecord.EmployeeId,
                Date = overtimeRecord.Date,
                Hours = overtimeRecord.Hours,
                Type = overtimeRecord.Type,
                IsApproved = overtimeRecord.IsApproved
            };
        }
    }
}
