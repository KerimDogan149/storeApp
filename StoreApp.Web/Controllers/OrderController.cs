using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Concrete;
using StoreApp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using StoreApp.Web.Helpers;
using StoreApp.Web.Pages;
using System.Security.Claims;



namespace StoreApp.Web.Controllers
{

[Authorize]
public class OrderController : Controller
{
    private readonly IStoreRepository _repository;
    private readonly UserManager<AppUser> _userManager;

    public OrderController(IStoreRepository repository, UserManager<AppUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();

        var userId = _userManager.GetUserId(User);
        var addresses = await _repository.GetAddressesByUserIdAsync(userId);

        var model = new OrderCheckoutViewModel
        {
            Cart = cart,
            Addresses = addresses
        };

        return View(model);
    }

    [HttpPost]

    public async Task<IActionResult> Checkout(int SelectedAddressId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var address = await _repository.GetAddressByIdAsync(SelectedAddressId);
        if (address == null || address.AppUserId != userId)
        {
            return RedirectToAction("Checkout");
        }

        var cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        if (cart.Items.Count == 0)
        {
            return RedirectToAction("Checkout");
        }

        // Siparişi oluştur
        var order = new Order
        {
            AppUserId = userId,
            AddressId = SelectedAddressId,
            OrderDate = DateTime.Now,
            TotalAmount = cart.CalculateTotal(),
            OrderItems = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.Product.Id,
                Quantity = i.Quantity,
                Price = i.Product.Price
            }).ToList()
        };

        await _repository.CreateOrderAsync(order);

        // Sepeti temizle
        HttpContext.Session.Remove("Cart");

        // Başarılı sayfasına yönlendir
        return RedirectToAction("Success", new { id = order.Id });
    }


        [HttpGet]
        public async Task<IActionResult> GetAddressDetails(int id)
        {
        var address = await _repository.Addresses.FirstOrDefaultAsync(a => a.Id == id);

            if (address == null)
            {
                return Content("Adres bulunamadı.");
            }
            return PartialView("_AddressDetailsPartial", address);
        }


            public async Task<IActionResult> Success(int id)
            {
                var order = await _repository.Orders
                    .Include(o => o.Address)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return NotFound();

                return View(order);
            }
}
}