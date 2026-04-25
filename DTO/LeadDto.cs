using FastLead.Enums;

namespace FastLead.DTO
{
    public class LeadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public LeadStatus Status { get; set; }
    }
}
