using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Web.Models;
using StoreApp.Data.Abstract;
using StoreApp.Data.Concrete;
using StoreApp.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StoreApp.Web.Components
{
    public class SimilarProductsViewComponent : ViewComponent
    {
        private readonly IStoreRepository _repository;

    public SimilarProductsViewComponent(IStoreRepository repository)
    {
        _repository = repository;
    }

    public IViewComponentResult Invoke(int productId)
    {
        var product = _repository.Products
            .Include(p => p.ProductCategories)
            .FirstOrDefault(p => p.Id == productId);

        if (product == null) return View(new List<Product>());

        var categoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList();

        var similarProducts = _repository.Products
            .Include(p => p.ProductCategories)
            .Where(p => p.Id != productId && p.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId)))
            .Take(4)
            .ToList();

        return View(similarProducts);
    }
    }
}