using FastLead.Interfaces;
using FastLead.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FastLead.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task CreateAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var account = await GetByIdAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task BulkDelete(List<Guid> ids)
        {
            var idsToDelete = _context.Accounts.Where(x => ids.Contains(x.Id)).ToList();
            _context.Accounts.RemoveRange(idsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Account>> GetRangeAsync(List<Guid> ids)
        {
            var accs = await _context.Accounts.Where(x => ids.Contains(x.Id)).ToListAsync();
            return accs;
        }

        public async Task<List<Guid>> GetFiltersAsync(string field, string value)
        {
            var query = _context.Accounts.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(value))
            {
                query = field switch
                {
                    "Name" => query.Where(a => a.Name.Contains(value)),
                    "INN" => query.Where(a => a.INN.Contains(value)),
                    "Email" => query.Where(a => a.Email.Contains(value)),
                    "Owner" => query.Where(a => a.Owner.Contains(value)),
                    "Phone" => query.Where(a => a.Phone.Contains(value)),
                    "Address" => query.Where(a => a.Address.Contains(value)),
                    "Type" => query.Where(a => a.Type.ToString().Contains(value)),
                    _ => query
                };
            }

            return await query.Select(a => a.Id).ToListAsync();
        }
    }
}
