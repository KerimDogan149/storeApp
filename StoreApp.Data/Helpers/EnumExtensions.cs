using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StoreApp.Data.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())[0]
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name ?? enumValue.ToString();
        }


        public static string GetOrderStatusBadgeClass(this OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Hazirlaniyor => "bg-warning text-dark",
                OrderStatus.KargoyaVerildi => "bg-info text-dark",
                OrderStatus.TeslimEdildi => "bg-success",
                OrderStatus.IadeTalebinde => "bg-secondary",
                OrderStatus.IadeEdildi => "bg-dark text-white",
                OrderStatus.IptalEdildi => "bg-danger",
                _ => "bg-light text-dark"

            };
        }



    }
}