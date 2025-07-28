using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Web.Models;
using StoreApp.Data.Abstract;
using StoreApp.Data.Concrete;
using StoreApp.Web.Helpers;
using Microsoft.AspNetCore.Mvc;


namespace StoreApp.Web.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return View(cart);
        }



    }

}