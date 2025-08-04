using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreApp.Web.Areas.Admin.Models.Reports
{
    public class ProductSalesListViewModel
    {
        public List<ProductSalesListItemViewModel> Products { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string FilterName { get; set; }
        public int? FilterCategoryId { get; set; }
        public string SortOrder { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}