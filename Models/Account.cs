using FastLead.Enums;
using FastLead.Interfaces;

namespace FastLead.Models
{
    public class Account : IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Owner { get; set; }
        public string Address { get; set; }
        public string INN { get; set; }
        public string Phone { get; set; }
        public AccountType Type { get; set; } = AccountType.Client;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "System";
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "System";
    }
}
