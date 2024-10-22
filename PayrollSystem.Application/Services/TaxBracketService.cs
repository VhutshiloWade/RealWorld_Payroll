using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Services
{
    public class TaxBracketService : ITaxBracketService
    {
        private readonly ITaxBracketRepository _taxBracketRepository;

        public TaxBracketService(ITaxBracketRepository taxBracketRepository)
        {
            _taxBracketRepository = taxBracketRepository;
        }

        public async Task<IEnumerable<TaxBracketDto>> GetAllTaxBracketsAsync()
        {
            var taxBrackets = await _taxBracketRepository.GetAllAsync();
            return taxBrackets.Select(t => MapToDto(t));
        }

        public async Task<TaxBracketDto> GetTaxBracketByIdAsync(int id)
        {
            var taxBracket = await _taxBracketRepository.GetByIdAsync(id);
            return taxBracket != null ? MapToDto(taxBracket) : null;
        }

        public async Task<TaxBracketDto> CreateTaxBracketAsync(TaxBracketDto taxBracketDto)
        {
            var taxBracket = MapToEntity(taxBracketDto);
            await _taxBracketRepository.AddAsync(taxBracket);
            return MapToDto(taxBracket);
        }

        public async Task UpdateTaxBracketAsync(int id, TaxBracketDto taxBracketDto)
        {
            var taxBracket = await _taxBracketRepository.GetByIdAsync(id);
            if (taxBracket == null)
            {
                throw new KeyNotFoundException($"Tax Bracket with ID {id} not found.");
            }

            UpdateEntityFromDto(taxBracket, taxBracketDto);
            await _taxBracketRepository.UpdateAsync(taxBracket);
        }

        public async Task DeleteTaxBracketAsync(int id)
        {
            await _taxBracketRepository.DeleteAsync(id);
        }

        private TaxBracketDto MapToDto(TaxBracket taxBracket)
        {
            return new TaxBracketDto
            {
                Id = taxBracket.Id,
                LowerLimit = taxBracket.LowerLimit,
                UpperLimit = taxBracket.UpperLimit,
                Rate = taxBracket.Rate
            };
        }

        private TaxBracket MapToEntity(TaxBracketDto dto)
        {
            return new TaxBracket
            {
                LowerLimit = dto.LowerLimit,
                UpperLimit = dto.UpperLimit,
                Rate = dto.Rate
            };
        }

        private void UpdateEntityFromDto(TaxBracket taxBracket, TaxBracketDto dto)
        {
            taxBracket.LowerLimit = dto.LowerLimit;
            taxBracket.UpperLimit = dto.UpperLimit;
            taxBracket.Rate = dto.Rate;
        }
    }
}
