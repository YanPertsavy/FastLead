using FastLead.DTO;
using FastLead.Models;

namespace FastLead.Interfaces
{
    public interface ILeadRepository
    {
        Task<List<Lead>> GetAllAsync();
        Task<Lead?> GetByIdAsync(Guid id);
        Task CreateAsync(Lead lead);
        Task UpdateAsync(Lead lead);
        Task DeleteAsync(Guid id);
        Task BulkDelete(List<Guid> ids);
        Task<List<Lead>> GetRangeAsync(List<Guid> ids);
        Task<List<LeadDto>> GetFiltersAsync(string field, string value);
        Task<List<LeadDto>> GetAllDtoAsync();
        Task<DashboardDTO> GetDashboardDTOAsync();
    }
}
