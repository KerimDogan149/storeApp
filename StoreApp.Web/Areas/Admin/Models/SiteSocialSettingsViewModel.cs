using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace StoreApp.Web.Areas.Admin.Models
{
    public class SiteSocialSettingsViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Facebook Linki")]
        public string FacebookUrl { get; set; }

        [Display(Name = "Instagram Linki")]
        public string InstagramUrl { get; set; }

        [Display(Name = "Twitter Linki")]
        public string TwitterUrl { get; set; }

        [Display(Name = "YouTube Linki")]
        public string YoutubeUrl { get; set; }

    }
}