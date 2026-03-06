using FastLead.Enums;

namespace FastLead.DTO
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AccountType Type { get; set; }
        public string Address { get; set; }
        public string INN { get; set; }
    }
}