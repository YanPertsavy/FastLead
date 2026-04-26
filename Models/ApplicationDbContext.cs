using FastLead.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;

namespace FastLead.Models
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            Database.EnsureCreated();
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries()
                .Where(e => (e.Entity is IAuditable) && (e.State == EntityState.Modified || e.State == EntityState.Added));

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var name = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value ?? "System";

            foreach (var entity in entities)
            {
                var audit = (IAuditable)entity.Entity;
                audit.ModifiedBy = name;
                audit.ModifiedOn = DateTime.UtcNow;
                if (entity.State == EntityState.Added)
                {
                    audit.CreatedBy = name;
                    audit.CreatedOn = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Debt> Debts { get; set; }
    }
}