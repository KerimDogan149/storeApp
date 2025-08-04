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

            var updatedItems = Cart.Items
                .Where(i =>
                {
                    var product = _repository.Products.FirstOrDefault(p => p.Id == i.Product.Id);
                    return product != null && product.Stock > 0;
                })
                .ToList();

            if (updatedItems.Count != Cart.Items.Count)
            {
                Cart.Items = updatedItems;
                HttpContext.Session.SetJson("Cart", Cart);
                TempData["ErrorMessage"] = "Stokta kalmayan ürünler sepetten çıkarıldı.";
            }
        }


        public IActionResult OnPost(int Id)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == Id);

            if (product == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı.";
                return RedirectToPage("/Cart");
            }

            Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();

            var existingItem = Cart.Items.FirstOrDefault(l => l.Product.Id == Id);
            int existingQuantity = existingItem?.Quantity ?? 0;

            if (existingQuantity + 1 > product.Stock)
            {
                TempData["ErrorMessage"] = $"{product.Name} ürününden stokta sadece {product.Stock} adet var.";
                return RedirectToPage("/Cart");
            }

            Cart.AddItem(product, 1);
            HttpContext.Session.SetJson("Cart", Cart);

            TempData["SuccessMessage"] = $"{product.Name} sepete eklendi.";
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
                var product = _repository.Products.FirstOrDefault(p => p.Id == Id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Ürün bulunamadı.";
                    return RedirectToPage();
                }

                if (item.Quantity >= product.Stock)
                {
                    TempData["ErrorMessage"] = $"{product.Name} ürününden stokta yeterince yok.";
                    return RedirectToPage();
                }

                item.Quantity++;
                HttpContext.Session.SetJson("Cart", Cart);
                TempData["SuccessMessage"] = $"{product.Name} adedi artırıldı.";
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