using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;

namespace StoreApp.Web.Components
{
    public class NavbarSearchViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

        
    }
}