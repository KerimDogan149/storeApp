using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;
using StoreApp.Data.Abstract;
using StoreApp.Web.Helpers;


namespace StoreApp.Web.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository _repository;
        public CartModel(IStoreRepository repository)
        {
            _repository = repository;
        }

        public Cart? Cart { get; set; }
        public void OnGet()
        {
            Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        }

        public IActionResult OnPost(int Id)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == Id);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson("Cart", Cart);
            }

            return RedirectToPage("/Cart");

        }

        public IActionResult OnPostRemoveFromCart(int Id)
        {
            Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            var product = Cart.Items.First(i => i.Product.Id == Id).Product;
            Cart?.RemoveItem(product);
            HttpContext.Session.SetJson("Cart", Cart);
            return RedirectToPage("/Cart");
        }
        public IActionResult OnPostIncreaseQuantity(int Id)
    {
        Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        var item = Cart.Items.FirstOrDefault(i => i.Product.Id == Id);
        if (item != null)
        {
            item.Quantity++;
            HttpContext.Session.SetJson("Cart", Cart);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostDecreaseQuantity(int Id)
    {
        Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        var item = Cart.Items.FirstOrDefault(i => i.Product.Id == Id);
        if (item != null)
        {
            if (item.Quantity > 1)
            {
                item.Quantity--;
            }
            else
            {
                Cart.RemoveItem(item.Product);
            }
            HttpContext.Session.SetJson("Cart", Cart);
        }
        return RedirectToPage();
    }

        public IActionResult OnPostClearCart()
        {
            Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            Cart.Clear();
            HttpContext.Session.SetJson("Cart", Cart);
            return RedirectToPage("/Cart");
        }
    }
}