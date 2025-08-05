using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;

namespace StoreApp.Data.Entities
{
    public class Favorite
    {

    public int Id { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}