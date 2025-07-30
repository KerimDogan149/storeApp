using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;
using StoreApp.Data.Abstract;


namespace StoreApp.Web.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            var item = Items.Where(p => p.Product.Id == product.Id).FirstOrDefault();

            if (item == null)
            {
                Items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void RemoveItem(Product product)
        {
            Items.RemoveAll(p => p.Product.Id == product.Id);
        }

        public decimal CalculateTotal()
        {
            return Items.Sum(item => item.Product.Price * item.Quantity);
        }

        public void Clear()
        {
            Items.Clear();
        }
    }

    public class CartItem
    {
        public int CartItemId { get; set; }

        public Product Product { get; set; } = new Product();

        public int Quantity { get; set; }
    }
}