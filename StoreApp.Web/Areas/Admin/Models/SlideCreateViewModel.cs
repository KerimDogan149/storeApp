using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StoreApp.Web.Areas.Admin.Models
{
    public class SlideCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Görsel")]
        public IFormFile? ImageFile { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "Link alanı zorunludur.")]
        [Display(Name = "Link")]
        public string? Link { get; set; }
        [Display(Name = "Aktif mi?")]

        public bool IsActive { get; set; } = true;
        [Display(Name = "Sıralama")]
        [Required(ErrorMessage = "Sıralama değeri zorunludur.")]
        public int Order { get; set; } = 0;

    }
}