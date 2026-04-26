using FastLead.Enums;
using FastLead.Interfaces;

namespace FastLead.Models
{
    public class Debt : IAuditable
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public bool IsOverdue { get; set; }
        public string ContractNumber { get; set; }
        public ServiceType ServiceType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
