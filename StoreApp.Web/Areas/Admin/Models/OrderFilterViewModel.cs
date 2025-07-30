using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Entities;
using StoreApp.Data.Concrete;

namespace StoreApp.Web.Areas.Admin.Models
{
    public class OrderFilterViewModel
    {
        public string? UserName { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinTotal { get; set; }
        public decimal? MaxTotal { get; set; }

        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
    }
}