using FastLead.Models;
using System.Linq.Expressions;

namespace FastLead.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(Guid id);
        Task CreateAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(Guid id);
        Task BulkDelete(List<Guid> ids);
        Task<List<Account>> GetRangeAsync(List<Guid> ids);
        Task<List<Guid>> GetFiltersAsync(string field, string value);
    }
}
