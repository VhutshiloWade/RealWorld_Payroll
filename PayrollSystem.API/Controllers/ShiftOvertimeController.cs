using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;

namespace PayrollSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftOvertimeController : ControllerBase
    {
        private readonly IShiftOvertimeService _shiftOvertimeService;

        public ShiftOvertimeController(IShiftOvertimeService shiftOvertimeService)
        {
            _shiftOvertimeService = shiftOvertimeService;
        }

        [HttpPost("shifts")]
        public async Task<ActionResult<ShiftDto>> CreateShift(ShiftDto shiftDto)
        {
            var result = await _shiftOvertimeService.CreateShiftAsync(shiftDto);
            return CreatedAtAction(nameof(GetEmployeeShifts), new { employeeId = result.EmployeeId }, result);
        }

        [HttpPut("shifts/{id}")]
        public async Task<ActionResult<ShiftDto>> UpdateShift(int id, ShiftDto shiftDto)
        {
            try
            {
                var result = await _shiftOvertimeService.UpdateShiftAsync(id, shiftDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("shifts/{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            await _shiftOvertimeService.DeleteShiftAsync(id);
            return NoContent();
        }

        [HttpGet("shifts/{employeeId}")]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetEmployeeShifts(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _shiftOvertimeService.GetEmployeeShiftsAsync(employeeId, startDate, endDate);
            return Ok(result);
        }

        [HttpPost("overtime")]
        public async Task<ActionResult<OvertimeRecordDto>> CreateOvertimeRecord(OvertimeRecordDto overtimeRecordDto)
        {
            var result = await _shiftOvertimeService.CreateOvertimeRecordAsync(overtimeRecordDto);
            return CreatedAtAction(nameof(GetEmployeeOvertimeRecords), new { employeeId = result.EmployeeId }, result);
        }

        [Authorize(Roles = "Manager,HRAdmin")]
        [HttpPut("overtime/{id}/approve")]
        public async Task<ActionResult<OvertimeRecordDto>> ApproveOvertimeRecord(int id)
        {
            try
            {
                var result = await _shiftOvertimeService.ApproveOvertimeRecordAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("overtime/{employeeId}")]
        public async Task<ActionResult<IEnumerable<OvertimeRecordDto>>> GetEmployeeOvertimeRecords(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _shiftOvertimeService.GetEmployeeOvertimeRecordsAsync(employeeId, startDate, endDate);
            return Ok(result);
        }

        [HttpGet("overtime/{employeeId}/hours")]
        public async Task<ActionResult<decimal>> GetEmployeeOvertimeHours(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _shiftOvertimeService.CalculateOvertimeHoursAsync(employeeId, startDate, endDate);
            return Ok(result);
        }

        [HttpGet("shifts/{employeeId}/differential")]
        public async Task<ActionResult<decimal>> GetEmployeeShiftDifferential(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _shiftOvertimeService.CalculateShiftDifferentialAsync(employeeId, startDate, endDate);
            return Ok(result);
        }
    }
}
