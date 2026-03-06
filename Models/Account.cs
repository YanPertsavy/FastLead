using FastLead.Enums;

namespace FastLead.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Owner { get; set; }
        public string Address { get; set; }
        public string INN { get; set; }
        public string Phone { get; set; }
        public AccountType Type { get; set; } = AccountType.Client;

    }
}
