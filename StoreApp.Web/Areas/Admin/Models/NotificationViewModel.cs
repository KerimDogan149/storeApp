using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Web.Areas.Admin.Models
{
    public class NotificationViewModel
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public bool IsUnread { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Icon { get; set; }
        public string BadgeClass { get; set; }
    }
}