using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace StoreApp.Web.Models
{
    public class AddressViewModel
    {
        public int? Id { get; set; }

        [Required, Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Required, Phone, Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Required, Display(Name = "İl")]
        public string Province { get; set; }

        [Required, Display(Name = "İlçe")]
        public string District { get; set; }

        [Required, Display(Name = "Mahalle")]
        public string Neighborhood { get; set; }

        [Display(Name = "Detaylı Adres")]
        public string FullAddress { get; set; }

        [Display(Name = "Adres Başlığı")]
        public string Title { get; set; }

        public List<SelectListItem> Provinces { get; set; } = new();

        public List<SelectListItem> Districts { get; set; } = new();

        public List<SelectListItem> Neighborhoods { get; set; } = new();
        

        
    }
}