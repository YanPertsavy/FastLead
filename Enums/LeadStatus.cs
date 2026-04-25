using System.ComponentModel.DataAnnotations;

namespace FastLead.Enums
{
    public enum LeadStatus
    {
        [Display(Name = "Новый")]
        New = 0,
        [Display(Name = "В работе")]
        InProgress = 1,
        [Display(Name = "Квалифицирован")]
        Qualified = 2,
        [Display(Name = "Отклонён")]
        Rejected = 3,
        [Display(Name = "Конвертирован")]
        Converted = 4,
        [Display(Name = "Другое")]
        Other = 5
    }
}
