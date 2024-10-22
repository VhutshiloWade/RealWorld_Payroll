using PayrollSystem.Application.DTOs;

namespace PayrollSystem.Application.Interfaces
{
    public interface ITaxBracketService
    {
        Task<IEnumerable<TaxBracketDto>> GetAllTaxBracketsAsync();
        Task<TaxBracketDto> GetTaxBracketByIdAsync(int id);
        Task<TaxBracketDto> CreateTaxBracketAsync(TaxBracketDto taxBracketDto);
        Task UpdateTaxBracketAsync(int id, TaxBracketDto taxBracketDto);
        Task DeleteTaxBracketAsync(int id);
    }
}
