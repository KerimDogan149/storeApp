using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Web.Models;
using StoreApp.Data.Abstract;
using StoreApp.Data.Concrete;
using StoreApp.Data.Entities;
using StoreApp.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Entities;


namespace StoreApp.Web.Components
{
    public class SimilarProductsViewComponent : ViewComponent
    {
        private readonly IStoreRepository _repository;
            private readonly UserManager<AppUser> _userManager;

        public SimilarProductsViewComponent(IStoreRepository repository, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var product = _repository.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
                return View(new List<ProductViewModel>());

            var categoryIds = product.ProductCategories
                .Select(pc => pc.CategoryId)
                .ToList();

            var similarProducts = _repository.Products
                .Include(p => p.ProductCategories)
                .Where(p => p.Id != productId &&
                            p.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId)))
                .Take(4)
                .ToList();

            string userId = User.Identity.IsAuthenticated ? _userManager.GetUserId(User as System.Security.Claims.ClaimsPrincipal) : null;

            var viewModelList = new List<ProductViewModel>();

            foreach (var p in similarProducts)
            {
                bool isFavorited = false;

                if (userId != null)
                {
                    isFavorited = await _repository.IsProductFavoritedAsync(userId, p.Id);
                }

                viewModelList.Add(new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Url = p.Url,
                    Price = p.Price,
                    Image = p.Image,
                    IsBestSeller = p.IsBestSeller,
                    IsFeatured = p.IsFeatured,
                    IsFavorited = isFavorited
                });
            }

            return View(viewModelList);
        }
    }
}
