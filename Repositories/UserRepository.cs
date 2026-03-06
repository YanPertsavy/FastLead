namespace FastLead.Repositories
{
    using FastLead.Models;
    using FastLead.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Нахождение по ID
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        // 2. Добавление одного юзера
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // 3. Добавление списка юзеров
        public async Task AddRangeAsync(IEnumerable<User> users)
        {
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        // 4. Удаление (по объекту или по ID)
        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(string Name)
        {
            return await _context.Users.AnyAsync(u => u.Name == Name);
        }

        // Дополнительный полезный метод для логина
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users?.FirstOrDefaultAsync(u => u.Name == username);
        }
    }
}
