using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StoreApp.Data.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.Web.Areas.Admin.Models
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        public string Name { get; set; }

        public string? Url { get; set; }

        [Required(ErrorMessage = "Fiyat alanı zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        public string Description { get; set; }

        public string? Image { get; set; }
        [Display(Name = "Öne Çıkan Ürün")]
        public bool IsFeatured { get; set; } = false;
        [Display(Name = "En Çok Satan Ürün")]
        public bool IsBestSeller { get; set; } = false;
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "En az bir kategori seçilmelidir.")]
        public List<int> SelectedCategoryIds { get; set; } = new();


        public List<Category>? AllCategories { get; set; }
    }
}
