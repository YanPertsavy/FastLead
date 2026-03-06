using FastLead.Models;

namespace FastLead.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User entity);
        Task AddRangeAsync(IEnumerable<User> users);
        Task DeleteAsync(Guid id);
        Task Update(User entity);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> Exists(string Name);
    }
}
