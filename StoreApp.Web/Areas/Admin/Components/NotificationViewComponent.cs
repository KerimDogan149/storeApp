using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Concrete;
using System.Threading.Tasks;
using StoreApp.Web.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreApp.Web.Areas.Admin.Components
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly StoreDbContext _context;

        public NotificationViewComponent(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = new List<NotificationViewModel>();

            var threshold = 5;
            var criticalCount = await _context.Products.CountAsync(p => p.Stock <= threshold);
            if (criticalCount > 0)
            {
                notifications.Add(new NotificationViewModel
                {
                    Type = "stock",
                    Message = $"{criticalCount} ürün kritik stok seviyesinde.",
                    Url = "/Admin/ProductStock",
                    IsUnread = true,
                    Icon = "bi-exclamation-triangle-fill",
                    BadgeClass = "bg-danger",
                    CreatedAt = DateTime.Now
                });
            }


            var newOrderCount = await _context.Orders.CountAsync(o => !o.IsSeen);
            if (newOrderCount > 0)
            {
                notifications.Add(new NotificationViewModel
                {
                    Type = "order",
                    Message = $"{newOrderCount} yeni sipariş var.",
                    Url = "/Admin/Order",
                    IsUnread = true,
                    Icon = "bi-bag-check-fill",
                    BadgeClass = "bg-warning",
                    CreatedAt = DateTime.Now
                });
            }

            return View(notifications);
        }
    }
}
