using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Helpers;
using StoreApp.Data.Concrete;
using StoreApp.Data.Entities;

namespace StoreApp.Web.Models
{
    public class UserOrdersFilterViewModel
    {
        public OrderStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinTotal { get; set; }
        public decimal? MaxTotal { get; set; }

        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
    }
}