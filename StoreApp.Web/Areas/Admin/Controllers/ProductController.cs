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
    [Authorize (Roles = "Admin")]


    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IStoreRepository _repository;

        public ProductController(IStoreRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(int? categoryId, string search,string isFeatured, string isBestSeller)
        {
            var products = _repository.Products.Include(p => p.Categories).AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.Categories.Any(c => c.Id == categoryId.Value));
            }
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }
            
                if (!string.IsNullOrEmpty(isFeatured))
                {
                    bool featured = isFeatured == "true";
                    products = products.Where(p => p.IsFeatured == featured);
                }

                if (!string.IsNullOrEmpty(isBestSeller))
                {
                    bool bestSeller = isBestSeller == "true";
                    products = products.Where(p => p.IsBestSeller == bestSeller);
                }

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }

            var categories = _repository.Categories.ToList();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Search = search;
            ViewBag.IsFeaturedFilter = isFeatured;
            ViewBag.IsBestSellerFilter = isBestSeller;


            return View(products.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductCreateViewModel
            {
                AllCategories = _repository.Categories.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (model.SelectedCategoryIds == null || !model.SelectedCategoryIds.Any())
            {
                ModelState.AddModelError("SelectedCategoryIds", "En az bir kategori seçilmelidir.");
            }

            if (model.ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg, .png veya .gif uzantılı görseller yüklenebilir.");
                }

                if (model.ImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Görsel boyutu 2 MB'tan büyük olamaz.");
                }
            }
            if (ModelState.IsValid)
            {
                var url = model.Name.ToUrlSlug();
                if (model.Url != url)
                {
                    ModelState.AddModelError("Url", "URL sistem tarafından oluşturulmalıdır.");
                    model.Url = url;
                    model.AllCategories = _repository.Categories.ToList();
                    return View(model);
                }

                var product = new Product
                {
                    Name = model.Name,
                    Url = url,
                    Price = model.Price,
                    Description = model.Description,
                    IsFeatured = model.IsFeatured,
                    IsBestSeller = model.IsBestSeller,
                    Image = "default.png",
                    Categories = new List<Category>()
                };

                foreach (var catId in model.SelectedCategoryIds)
                {
                    var category = _repository.Categories.FirstOrDefault(c => c.Id == catId);
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }

                if (model.ImageFile != null)
                {
                    var ext = Path.GetExtension(model.ImageFile.FileName);
                    var imageName = $"{Guid.NewGuid()}{ext}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products", imageName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await model.ImageFile.CopyToAsync(stream);
                    product.Image = imageName;
                }

                await _repository.CreateProductAsync(product);
                return RedirectToAction("Index");
            }

            model.AllCategories = _repository.Categories.ToList();
            return View(model);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _repository.Products
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductCreateViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Url = product.Url,
                Price = product.Price,
                IsFeatured = product.IsFeatured,
                IsBestSeller = product.IsBestSeller,
                Description = product.Description,
                Image = product.Image,
                SelectedCategoryIds = product.Categories.Select(c => c.Id).ToList(),
                AllCategories = _repository.Categories.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCreateViewModel model)
        {
            if (model.SelectedCategoryIds == null || !model.SelectedCategoryIds.Any())
            {
                ModelState.AddModelError("SelectedCategoryIds", "En az bir kategori seçilmelidir.");
            }

            if (model.ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg, .png veya .gif uzantılı görseller yüklenebilir.");
                }

                if (model.ImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Görsel boyutu 2 MB'tan büyük olamaz.");
                }
            }

            if (ModelState.IsValid)
            {
                var product = _repository.Products
                    .Include(p => p.Categories)
                    .FirstOrDefault(p => p.Id == model.Id);

                if (product == null)
                {
                    return NotFound();
                }

                var expectedUrl = model.Name.ToUrlSlug();
                if (model.Url != expectedUrl)
                {
                    ModelState.AddModelError("Url", "URL sistem tarafından oluşturulmalıdır.");
                    model.Url = expectedUrl;
                    model.AllCategories = _repository.Categories.ToList();
                    return View(model);
                }

                product.Name = model.Name;
                product.Url = expectedUrl;
                product.Price = model.Price;
                product.Description = model.Description;
                product.IsFeatured = model.IsFeatured;
                product.IsBestSeller = model.IsBestSeller;

                if (model.ImageFile != null)
                {
                    var ext = Path.GetExtension(model.ImageFile.FileName);
                    var imageName = $"{Guid.NewGuid()}{ext}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products", imageName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await model.ImageFile.CopyToAsync(stream);
                    product.Image = imageName;
                }

                product.Categories.Clear();
                foreach (var catId in model.SelectedCategoryIds)
                {
                    var category = _repository.Categories.FirstOrDefault(c => c.Id == catId);
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }

                await _repository.UpdateProductAsync(product);
                return RedirectToAction("Index");
            }

            model.AllCategories = _repository.Categories.ToList();
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(product.Image) && product.Image != "default.png")
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products", product.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            await _repository.DeleteProductAsync(product.Id);
            return RedirectToAction("Index");
        }




    }

}