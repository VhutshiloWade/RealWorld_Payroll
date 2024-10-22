using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Interfaces
{
    public interface IShiftRepository
    {
        Task<Shift> GetByIdAsync(int id);
        Task<IEnumerable<Shift>> GetByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task AddAsync(Shift shift);
        Task UpdateAsync(Shift shift);
        Task DeleteAsync(int id);
    }
}
