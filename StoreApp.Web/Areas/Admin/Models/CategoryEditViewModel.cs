using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;
using System.ComponentModel.DataAnnotations;


namespace StoreApp.Web.Areas.Admin.Models
{
    public class CategoryEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [Display(Name = "Kategori Adı")]

        public string Name { get; set; } = string.Empty;
        [Display(Name = "Kategori URL'si")]
        public string Url { get; set; } = string.Empty;

        public string? Image { get; set; }

        public List<Product> AllProducts { get; set; } = new();

        public List<int> SelectedProductIds { get; set; } = new();
        
    }
}