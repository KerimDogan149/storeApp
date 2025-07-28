using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace StoreApp.Web.Areas.Admin.Models
{
public class LoginViewModel
{
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email alanı gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string Email { get; set; }

    [Display(Name = "Şifre")]
    [Required(ErrorMessage = "Şifre alanı gereklidir.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
}