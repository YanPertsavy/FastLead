using FastLead.Enums;
using FastLead.Interfaces;
using FastLead.Models;
using Microsoft.EntityFrameworkCore;

namespace FastLead.Repositories
{
    public class DebtRepository : IDebtRepository
    {
        private readonly ApplicationDbContext _context;

        public DebtRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Debt>> GetAllAsync()
            => await _context.Debts.Include(d => d.Account).ToListAsync();

        public async Task<Debt?> GetByIdAsync(Guid id)
            => await _context.Debts.Include(d => d.Account).FirstOrDefaultAsync(d => d.Id == id);

        public async Task AddAsync(Debt debt)
        {
            await _context.Debts.AddAsync(debt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Debt debt)
        {
            _context.Debts.Update(debt);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var debt = await GetByIdAsync(id);
            if (debt != null)
            {
                _context.Debts.Remove(debt);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var debts = await _context.Debts.Where(d => ids.Contains(d.Id)).ToListAsync();
            _context.Debts.RemoveRange(debts);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Debt>> GetFiltersAsync(string field, string value)
        {
            var query = _context.Debts.AsNoTracking().Include(d => d.Account).AsQueryable();

            if (!string.IsNullOrWhiteSpace(value))
            {
                query = field switch
                {
                    "ContractNumber" => query.Where(d => d.ContractNumber.Contains(value)),
                    "AccountName" => query.Where(d => d.Account.Name.Contains(value)),
                    "ServiceType" => Enum.TryParse<ServiceType>(value, true, out var typeResult)
                                        ? query.Where(d => d.ServiceType == typeResult)
                                        : query,
                    _ => query
                };
            }

            return await query.ToListAsync();
        }
    }
}
