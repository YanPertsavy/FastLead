using FastLead.Enums;

namespace FastLead.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; } = Role.User;

        public static User Create(Guid id, string name, string password)
        {
            return new User { Id = id, Name = name, Password = password };
        }
    }
}
