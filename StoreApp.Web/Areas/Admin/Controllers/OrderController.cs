using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Concrete;
using StoreApp.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.Data.Helpers;
using StoreApp.Web.Areas.Admin.Models;

namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {

        private readonly StoreDbContext _context;

        public OrderController(StoreDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(OrderFilterViewModel filter)
        {

            if (filter.StartDate.HasValue && filter.EndDate.HasValue)
            {
                if (filter.StartDate > filter.EndDate)
                {
                    ModelState.AddModelError("", "Başlangıç tarihi, bitiş tarihinden sonra olamaz.");
                }
            }
            if (filter.MinTotal.HasValue && filter.MaxTotal.HasValue)
            {
                if (filter.MinTotal > filter.MaxTotal)
                {
                    ModelState.AddModelError("", "Minimum tutar, maksimum tutardan büyük olamaz.");
                }
            }
            var query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                .Include(o => o.AppUser)
                .Include(o => o.Address)
                .AsQueryable();
            if (!string.IsNullOrEmpty(filter.UserName))
            {
                query = query.Where(o => o.AppUser.UserName.Contains(filter.UserName));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(o => o.Status == filter.Status);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= filter.EndDate.Value);
            }

            if (filter.MinTotal.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= filter.MinTotal.Value);
            }

            if (filter.MaxTotal.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= filter.MaxTotal.Value);
            }

            if (!string.IsNullOrEmpty(filter.ProductName))
            {
                var loweredName = filter.ProductName.ToLower();
                query = query.Where(o =>
                    o.OrderItems.Any(oi =>
                        oi.Product.Name.ToLower().Contains(loweredName)));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(o =>
                    o.OrderItems.Any(oi =>
                        oi.Product.ProductCategories.Any(pc => pc.CategoryId == filter.CategoryId)));
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Filter = filter;

            return View(orders);
        }



        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.AppUser)
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            ViewBag.Statuses = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = ((Enum)s).GetDisplayName(),
                    Selected = s == order.Status
                }).ToList();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, Order orderModel)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = orderModel.Status;

            _context.Update(order);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sipariş durumu başarıyla güncellendi.";
            return RedirectToAction("Details", new { id = order.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sipariş başarıyla silindi.";
            return RedirectToAction("Index");
        }





    }
}