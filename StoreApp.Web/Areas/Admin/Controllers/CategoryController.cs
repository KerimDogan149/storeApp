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

namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Authorize (Roles = "Admin")]

    [Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly IStoreRepository _repository;

        public CategoryController(IStoreRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var categories = _repository.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {

            if (ModelState.IsValid)
            {
                var expectedUrl = model.Name.ToUrlSlug();
                if (model.Url != expectedUrl)
                {
                    ModelState.AddModelError("Url", "Sisteme müdahale etmeyiniz.");
                    model.Url = model.Name.ToUrlSlug();
                    return View(model);
                }
                model.Url = model.Name.ToUrlSlug();
                model.Image = "empty.jpg";
                await _repository.CreateCategoryAsync(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

            [HttpGet]
            public IActionResult Edit(int id)
            {
                var category = _repository.Categories
                    .Include(c => c.Products)
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return NotFound();
                }

                var allProducts = _repository.Products.ToList();

                var model = new CategoryEditViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Url = category.Name.ToUrlSlug(),
                    Image = category.Image,
                    AllProducts = allProducts,
                    SelectedProductIds = category.Products.Select(p => p.Id).ToList()
                };

                return View(model);
            }


            [HttpPost]
            public async Task<IActionResult> Edit(CategoryEditViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var expectedUrl = model.Name.ToUrlSlug();
                    if (model.Url != expectedUrl)
                    {
                        ModelState.AddModelError("Url", "Sisteme müdahale etmeyiniz.");
                        model.Url = expectedUrl;
                        model.AllProducts = _repository.Products.ToList();
                        return View(model);
                    }

                    var category = _repository.Categories
                        .Include(c => c.Products)
                        .FirstOrDefault(c => c.Id == model.Id);

                    if (category == null)
                    {
                        return NotFound();
                    }

                        var removedProductIds = category.Products
                        .Select(p => p.Id)
                        .Except(model.SelectedProductIds)
                        .ToList();

                    foreach (var removedProductId in removedProductIds)
                    {
                        var product = _repository.Products
                            .Include(p => p.Categories)
                            .FirstOrDefault(p => p.Id == removedProductId);

                        if (product != null && product.Categories.Count <= 1)
                        {
                            ModelState.AddModelError("", $"'{product.Name}' ürünü yalnızca bu kategoriye sahiptir. Bu ürünü kategorisiz bırakamazsınız.");
                            model.AllProducts = _repository.Products.ToList();
                            model.SelectedProductIds = category.Products.Select(p => p.Id).ToList();



                            return View(model);
                        }
                    }

                    category.Name = model.Name;
                    category.Url = expectedUrl;
                    category.Image = "default.png";

                    category.Products.Clear();
                    foreach (var productId in model.SelectedProductIds)
                    {
                        var product = _repository.Products.FirstOrDefault(p => p.Id == productId);
                        if (product != null)
                        {
                            category.Products.Add(product);
                        }
                    }

                    await _repository.UpdateCategoryAsync(category);
                    return RedirectToAction("Index");
                }

                model.AllProducts = _repository.Products.ToList(); 
                return View(model);
            }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = _repository.Categories
                                    .Include(c => c.Products)
                                    .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            if (category.Products.Any())
            {
                TempData["error"] = "Bu kategoriye bağlı ürünler var. Önce o ürünleri silmelisiniz.";
                return RedirectToAction("Index");
            }

            await _repository.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }



    }

}