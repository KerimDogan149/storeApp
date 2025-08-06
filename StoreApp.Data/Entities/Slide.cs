using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace StoreApp.Data.Entities
{
    public class Slide
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Görsel alanı zorunludur.")]
        public string ImageUrl { get; set; } = null!;

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(200)]
        public string? Link { get; set; }

        public bool IsActive { get; set; } = true;

        public int Order { get; set; } = 0;

    }
}