using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Web.Areas.Admin.Models.Reports
{
    public class TopSellingProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Url { get; set; }
        public string ProductUrl { get; set; }
        public int QuantitySold { get; set; }
    }
}