using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Entities;

namespace StoreApp.Data.Concrete
{
public class Order
{
    public int Id { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public int AddressId { get; set; }   
    public Address Address { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    public decimal TotalAmount { get; set; }

    public string? Note { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Hazirlaniyor;


    public List<OrderItem> OrderItems { get; set; } = new();
}

}