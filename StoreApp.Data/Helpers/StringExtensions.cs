using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Data.Helpers
{
    public static class StringExtensions
    {
        public static string ToUrlSlug(this string text)
        {
            return text
                .ToLower()
                .Replace("ç", "c").Replace("ğ", "g")
                .Replace("ı", "i").Replace("ö", "o")
                .Replace("ş", "s").Replace("ü", "u")
                .Replace("&", "and")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace(" ", "-")
                .Replace("--", "-")
                .Trim('-');
        }
    }
}