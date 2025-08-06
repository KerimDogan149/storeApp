using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using StoreApp.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using StoreApp.Data.Entities;
using StoreApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;
using StoreApp.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using StoreApp.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ProductStockController : Controller
    {

        private readonly StoreDbContext _context;

        public ProductStockController(StoreDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(ProductStockFilterViewModel filter)
        {

            if (filter.MinStock.HasValue && filter.MaxStock.HasValue && filter.MinStock > filter.MaxStock)
            {
                ViewBag.FilterError = "Minimum stok, maksimum stoktan büyük olamaz.";
            }

            var productsQuery = _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                string lowered = filter.Name.ToLower();
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(lowered));
            }


            if (!(filter.MinStock.HasValue && filter.MaxStock.HasValue && filter.MinStock > filter.MaxStock))
            {
                if (filter.MinStock.HasValue)
                    productsQuery = productsQuery.Where(p => p.Stock >= filter.MinStock.Value);
                if (filter.MaxStock.HasValue)
                    productsQuery = productsQuery.Where(p => p.Stock <= filter.MaxStock.Value);
            }


            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                string loweredCategory = filter.Category.ToLower();
                productsQuery = productsQuery.Where(p =>
                    p.ProductCategories.Any(pc => pc.Category.Name.ToLower() == loweredCategory));
            }


            if (filter.Sort == "desc")
                productsQuery = productsQuery.OrderByDescending(p => p.Stock);
            else
                productsQuery = productsQuery.OrderBy(p => p.Stock);


            var products = productsQuery.Select(p => new ProductStockUpdateViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Url = p.Url,
                ImageUrl = p.Image,
                Price = p.Price,
                Stock = p.Stock,
                CategoryNames = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
            }).ToList();


            int threshold = 5;
            if (Request.Query.ContainsKey("threshold"))
            {
                int.TryParse(Request.Query["threshold"], out threshold);
            }

            int criticalCount = products.Count(p => p.Stock <= threshold);



            ViewBag.Categories = _context.Categories
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            ViewBag.Filter = filter;
            ViewBag.CriticalCount = criticalCount;
            ViewBag.Threshold = threshold;
            return View(products);
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            var model = new ProductStockUpdateViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Url = product.Url,
                ImageUrl = product.Image,
                Price = product.Price,
                Stock = product.Stock,
                CategoryNames = product.ProductCategories
                                    .Select(pc => pc.Category.Name)
                                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Update([Bind("Id,Stock")] ProductStockUpdateViewModel model)

        {

            if (!ModelState.IsValid)
            {
                var product = _context.Products
                    .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                    .FirstOrDefault(p => p.Id == model.Id);

                if (product == null)
                    return NotFound();

                model.Name = product.Name;
                model.Url = product.Url;
                model.ImageUrl = product.Image;
                model.Price = product.Price;
                model.CategoryNames = product.ProductCategories
                                            .Select(pc => pc.Category.Name)
                                            .ToList();

                return View(model);
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == model.Id);
            if (existingProduct == null)
                return NotFound();

            existingProduct.Stock = model.Stock;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Stok başarıyla güncellendi.";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult GetProductTablePartial(string? name, int? minStock, int? maxStock, string? category, string? sort = "asc", int? threshold = null)
        {
            var productsQuery = _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lowered = name.ToLower();
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(lowered));
            }

            if (minStock.HasValue)
                productsQuery = productsQuery.Where(p => p.Stock >= minStock.Value);

            if (maxStock.HasValue)
                productsQuery = productsQuery.Where(p => p.Stock <= maxStock.Value);

            if (!string.IsNullOrWhiteSpace(category))
            {
                string loweredCategory = category.ToLower();
                productsQuery = productsQuery.Where(p =>
                    p.ProductCategories.Any(pc => pc.Category.Name.ToLower() == loweredCategory));
            }

            if (sort == "desc")
                productsQuery = productsQuery.OrderByDescending(p => p.Stock);
            else
                productsQuery = productsQuery.OrderBy(p => p.Stock);

            var products = productsQuery.Select(p => new ProductStockUpdateViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Url = p.Url,
                ImageUrl = p.Image,
                Price = p.Price,
                Stock = p.Stock,
                CategoryNames = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
            }).ToList();


            int t = threshold ?? 5;
            ViewBag.CriticalCount = products.Count(p => p.Stock <= t);
            ViewBag.Threshold = t;

            return PartialView("ProductTablePartial", products);
        }

        [HttpGet]
        public IActionResult GetCriticalCount(string? name, int? minStock, int? maxStock, string? category, int? threshold = null)
        {
            var query = _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lowered = name.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(lowered));
            }

            if (minStock.HasValue)
                query = query.Where(p => p.Stock >= minStock.Value);

            if (maxStock.HasValue)
                query = query.Where(p => p.Stock <= maxStock.Value);

            if (!string.IsNullOrWhiteSpace(category))
            {
                string loweredCategory = category.ToLower();
                query = query.Where(p => p.ProductCategories.Any(pc => pc.Category.Name.ToLower() == loweredCategory));
            }

            int t = threshold ?? 5;
            int count = query.Count(p => p.Stock <= t);

            return Json(new { count = count, threshold = t });
        }





    }
}