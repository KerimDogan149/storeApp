using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Web.Areas.Admin.Models.Reports
{
    public class ProductSalesListItemViewModel
    {

    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }

    public string Categories { get; set; }
    public int TotalQuantitySold { get; set; }
    }
}