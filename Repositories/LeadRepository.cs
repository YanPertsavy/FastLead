using FastLead.DTO;
using FastLead.Enums;
using FastLead.Interfaces;
using FastLead.Models;
using Microsoft.EntityFrameworkCore;

namespace FastLead.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly ApplicationDbContext _context;

        public LeadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Lead>> GetAllAsync()
        {
            return await _context.Leads.ToListAsync();
        }

        public async Task<Lead?> GetByIdAsync(Guid id)
        {
            return await _context.Leads.FindAsync(id);
        }

        public async Task CreateAsync(Lead lead)
        {
            await _context.Leads.AddAsync(lead);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lead lead)
        {
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var lead = await GetByIdAsync(id);
            if (lead != null)
            {
                _context.Leads.Remove(lead);
                await _context.SaveChangesAsync();
            }
        }

        public async Task BulkDelete(List<Guid> ids)
        {
            var leadsToDelete = _context.Leads.Where(x => ids.Contains(x.Id)).ToList();
            _context.Leads.RemoveRange(leadsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Lead>> GetRangeAsync(List<Guid> ids)
        {
            return await _context.Leads.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<LeadDto>> GetFiltersAsync(string field, string value)
        {
            var query = _context.Leads.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(value))
            {
                query = field switch
                {
                    "Name" => query.Where(l => l.Name.Contains(value)),
                    "Company" => query.Where(l => l.Company.Contains(value)),
                    "Email" => query.Where(l => l.Email.Contains(value)),
                    "Owner" => query.Where(l => l.Owner.Contains(value)),
                    "Phone" => query.Where(l => l.Phone.Contains(value)),
                    "Address" => query.Where(l => l.Address.Contains(value)),
                    "Status" => Enum.TryParse<LeadStatus>(value, true, out var statusResult)
                                ? query.Where(l => l.Status == statusResult)
                                : query,
                    _ => query
                };
            }

            return await query.Select(l => new LeadDto
            {
                Id = l.Id,
                Name = l.Name,
                Company = l.Company,
                Phone = l.Phone,
                Status = l.Status
            }).ToListAsync();
        }

        public async Task<List<LeadDto>> GetAllDtoAsync()
        {
            return await _context.Leads
                .AsNoTracking()
                .Select(l => new LeadDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Company = l.Company,
                    Phone = l.Phone,
                    Status = l.Status
                })
                .ToListAsync();
        }

        public async Task<DashboardDTO> GetDashboardDTOAsync()
        {
            string[] colName = Enum.GetNames<LeadStatus>();
            var groups = await _context.Leads.GroupBy(x => x.Status).Select(g => new { Key = g.Key, Count = g.Count() }).ToListAsync();
            int[] colHeight = groups.Select(x => x.Count).ToArray();
            return new DashboardDTO() { colHeigt = colHeight, colName = colName, };
        }
    }
}
