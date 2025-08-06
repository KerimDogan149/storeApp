using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Web.Areas.Admin.Models.Reports
{
    public class ProductSaleDetailItemViewModel
    {
    public int OrderId { get; set; }

    public string Email { get; set; }  

    public string UserName { get; set; }
    public DateTime OrderDate { get; set; }
    public int Quantity { get; set; }

    }
}