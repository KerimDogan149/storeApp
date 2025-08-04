using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Web.Areas.Admin.Models.Reports
{
    public class ProductSalesDetailsViewModel
    {
    public int ProductId { get; set; }
    public string Image { get; set; }

    public string ProductName { get; set; }
    public string Categories { get; set; }
    public List<ProductSaleDetailItemViewModel> Sales { get; set; }

    }
}