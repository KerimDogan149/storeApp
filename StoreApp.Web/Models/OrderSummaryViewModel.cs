using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;
using StoreApp.Data.Entities;


namespace StoreApp.Web.Models
{
    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string AddressTitle { get; set; }

        public OrderStatus Status { get; set; }
        public Address Address { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}