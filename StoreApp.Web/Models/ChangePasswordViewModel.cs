using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.Web.Models
{
    public class ChangePasswordViewModel
    {
    [Required(ErrorMessage = "Mevcut şifre zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mevcut Şifre")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Yeni şifre tekrar zorunludur.")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Yeni şifreler eşleşmiyor.")]
    [Display(Name = "Yeni Şifre Tekrar")]
    public string ConfirmNewPassword { get; set; }

    }
}