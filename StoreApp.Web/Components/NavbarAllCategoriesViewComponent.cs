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
    public class NavbarAllCategoriesViewComponent : ViewComponent
    {
        private readonly StoreDbContext _context;

        public NavbarAllCategoriesViewComponent(StoreDbContext context)
        {
            _context = context;
        }
        
        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories
                .OrderBy(c => c.Name)
                .ToList();

            return View(categories);
        }




    }

}