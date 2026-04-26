using FastLead.Models;

namespace FastLead.Interfaces
{
    public interface IDebtRepository
    {
        Task<IEnumerable<Debt>> GetAllAsync();
        Task<Debt?> GetByIdAsync(Guid id);
        Task AddAsync(Debt debt);
        Task UpdateAsync(Debt debt);
        Task DeleteAsync(Guid id);
        Task DeleteRangeAsync(IEnumerable<Guid> ids);
        Task<List<Debt>> GetFiltersAsync(string field, string value);
    }
}
