using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Interfaces
{
    public interface IOvertimeRecordRepository
    {
        Task<OvertimeRecord> GetByIdAsync(int id);
        Task<IEnumerable<OvertimeRecord>> GetByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task AddAsync(OvertimeRecord overtimeRecord);
        Task UpdateAsync(OvertimeRecord overtimeRecord);
        Task DeleteAsync(int id);
    }
}
