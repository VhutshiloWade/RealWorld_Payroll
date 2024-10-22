using PayrollSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Interfaces
{
    public interface ITaxBracketRepository
    {
        Task<IEnumerable<TaxBracket>> GetAllAsync();
        Task<TaxBracket> GetByIdAsync(int id);
        Task AddAsync(TaxBracket taxBracket);
        Task UpdateAsync(TaxBracket taxBracket);
        Task DeleteAsync(int id);
    }
}
