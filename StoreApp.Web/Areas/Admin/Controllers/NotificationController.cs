using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using StoreApp.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using StoreApp.Data.Entities;
using StoreApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;
using StoreApp.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using StoreApp.Web.Areas.Admin.Models;

namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NotificationController : Controller
    {
        private readonly StoreDbContext _context;

        public NotificationController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> MarkOrdersAsSeen()
        {
            var unseenOrders = await _context.Orders
                .Where(o => !o.IsSeen)
                .ToListAsync();

            foreach (var order in unseenOrders)
            {
                order.IsSeen = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }

}