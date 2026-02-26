using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.UI.Models
{
    //список возможносных должностей в программе:
    //коротко: тип данных (enum)
    public enum UserRole
    {
        //админ -> полный доступ
        Admin,
        //сотрудник -> обычный доступ
        Employee,
        //поставщик -> ограниченный доступ
        Supplier
    }
}
//под капотом такая красота:
//админ = 0
//сотрудник = 1
//поставщик = 2
