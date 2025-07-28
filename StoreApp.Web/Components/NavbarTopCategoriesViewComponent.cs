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
    public class NavbarTopCategoriesViewComponent : ViewComponent
    {
        private readonly StoreDbContext _context;

        public NavbarTopCategoriesViewComponent(StoreDbContext context)
        {
            _context = context;
        }
        
        public IViewComponentResult Invoke()
        {
            var selectedCategoryIds = new List<int> { 1, 2, 3, 4 };

            var categories = _context.Categories
                .Where(c => selectedCategoryIds.Contains(c.Id))
                .ToList();

            return View(categories);
        }




    }

}