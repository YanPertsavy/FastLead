using System.ComponentModel.DataAnnotations;

namespace FastLead.Enums
{
    public enum AccountType
    {
        [Display(Name = "Клиент")]
        Client = 0,

        [Display(Name = "Партнер")]
        Partner = 1,

        [Display(Name = "Поставщик")]
        Supplier = 2,

        [Display(Name = "Конкурент")]
        Competitor = 3,

        [Display(Name = "Перспективный")]
        Prospect = 4,

        [Display(Name = "Другое")]
        Other = 5
    }
}
