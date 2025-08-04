using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StoreApp.Web.Areas.Admin.Models
{
    public class ProductBulkUploadViewModel
    {
        [Display(Name = "Ürün Excel Dosyası (.xlsx)")]
        [Required(ErrorMessage = "Lütfen bir Excel dosyası seçin.")]
        public IFormFile File { get; set; }

    }
}