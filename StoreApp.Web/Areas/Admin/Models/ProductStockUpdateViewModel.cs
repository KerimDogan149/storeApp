using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace StoreApp.Web.Areas.Admin.Models
{
    public class ProductStockUpdateViewModel
    {
     public int Id { get; set; }

    [Display(Name = "Ürün Adı")]
    
    public string? Name { get; set; }

    [Display(Name = "URL")]
    public string? Url { get; set; }

    [Display(Name = "Resim")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Fiyat")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Display(Name = "Kategori")]
    public List<string> CategoryNames { get; set; } = new();

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stok 0 veya daha büyük olmalıdır.")]
    [Display(Name = "Stok")]
    public int Stock { get; set; }
    }
}