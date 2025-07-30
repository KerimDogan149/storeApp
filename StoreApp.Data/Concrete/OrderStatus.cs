using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.Data.Concrete
{
    public enum OrderStatus
    {
        [Display(Name = "Hazırlanıyor")]
        Hazirlaniyor = 0,

        [Display(Name = "Kargoya Verildi")]
        KargoyaVerildi = 1,

        [Display(Name = "Teslim Edildi")]
        TeslimEdildi = 2,

        [Display(Name = "İade Talebinde")]
        IadeTalebinde = 3,

        [Display(Name = "İade Edildi")]
        IadeEdildi = 4
    }
}