using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;

namespace StoreApp.Web.Components
{
    public class CategoriesListViewComponent : ViewComponent
    {
        private readonly IStoreRepository _storeRepository;
        public CategoriesListViewComponent(IStoreRepository storeRepository)
        {
            _storeRepository= storeRepository;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"]?.ToString();
 
            return View(_storeRepository.Categories.Select(p=> new CategoryViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Url = p.Url,
                Image = p.Image 
            }).ToList()); 
        }
    }
}