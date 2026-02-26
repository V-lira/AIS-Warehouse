using System.Collections.ObjectModel;

namespace AIS.Warehouse.UI.ViewModels
{
    public class AboutCompanyViewModel
    {
        public string CompanyName { get; }
        public string Address { get; }
        public string INN { get; }
        public string Phone { get; }
        public string Email { get; }
        public string Description { get; }

        public AboutCompanyViewModel()
        {
            CompanyName = "ООО «КРУТОЕ_НАЗВАНИЕ»";
            Address = "г. Москва, ул. Складская, д. 15, офис 304";
            INN = "7701234567";
            Phone = "+7 (495) 123-45-67";
            Email = "info@zolotoy-yashik.ru";
            Description = "Производство упаковочных материалов и складского оборудования. " +
                         "Основные направления: деревянная и пластиковая тара, стеллажные системы, " +
                         "упаковочные решения для логистики.";
        }
    }
}