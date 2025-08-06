using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace StoreApp.Web.Areas.Admin.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class CampaignViewModel
    {
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Alt Başlık")]
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]

        public string? SubTitle { get; set; }

        [Display(Name = "URL")]
        [Required(ErrorMessage = "URL alanı zorunludur.")]
        public string? Url { get; set; }

        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        public string? Description { get; set; }

        [Display(Name = "Buton Linki")]
        public string? Link { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;


        [Display(Name = "Görsel")]
        public IFormFile? ImageFile { get; set; }
    }

}