using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Web.Areas.Admin.Models
{
    public class ProductStockFilterViewModel
    {
        public string? Name { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }

        public string? Category { get; set; }

         public string? Sort { get; set; }

        
    }
}