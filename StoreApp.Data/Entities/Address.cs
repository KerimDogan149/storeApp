using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApp.Data.Entities
{
    public class Address
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Display(Name = "Cep Telefonu")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "İl seçimi zorunludur.")]
        [Display(Name = "İl")]
        public string Province { get; set; }

        [Required(ErrorMessage = "İlçe seçimi zorunludur.")]
        [Display(Name = "İlçe")]
        public string District { get; set; }

        [Required(ErrorMessage = "Mahalle seçimi zorunludur.")]
        [Display(Name = "Mahalle")]
        public string Neighborhood { get; set; }

        [Display(Name = "Adres")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Adres alanı zorunludur.")]
        public string FullAddress { get; set; }

        [Display(Name = "Adres Başlığı")]
        [Required(ErrorMessage = "Adres başlığı zorunludur.")]
        public string Title { get; set; }

        [Required]
        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

    }
}