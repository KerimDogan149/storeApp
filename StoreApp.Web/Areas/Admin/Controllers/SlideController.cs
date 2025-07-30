using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
using StoreApp.Data.Entities;
using StoreApp.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using StoreApp.Web.Areas.Admin.Models;
using StoreApp.Data.Helpers;
using Microsoft.EntityFrameworkCore;



namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles = "Admin")]


    public class SlideController : Controller
    {
        private readonly IStoreRepository _repository;
        private readonly IWebHostEnvironment _env;

        public SlideController(IStoreRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

[HttpGet]
public async Task<IActionResult> Index(string? status, int? order)
{
    var slides = await _repository.GetAllSlidesAsync();

    if (!string.IsNullOrEmpty(status))
    {
        if (status == "active")
            slides = slides.Where(s => s.IsActive).ToList();
        else if (status == "inactive")
            slides = slides.Where(s => !s.IsActive).ToList();
    }

    if (order.HasValue)
    {
        slides = slides.Where(s => s.Order == order.Value).ToList();
    }

    var orderedSlides = slides
        .OrderByDescending(s => s.IsActive)
        .ThenBy(s => s.Order)
        .ToList();

    return View(orderedSlides);
}


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SlideCreateViewModel model)
        {
            if (model.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Görsel dosyası yüklenmelidir.");
            }

            var generatedSlug = "/" + model.Title?.ToUrlSlug();


            if (!string.IsNullOrEmpty(model.Link) && model.Link.ToUrlSlug() != generatedSlug)
            {
                ModelState.AddModelError("Link", "Link ile başlıktan üretilen URL uyuşmuyor.");
                model.Link = generatedSlug;
            }

            var existing = await _repository.Slides.AnyAsync(s => s.Link == generatedSlug);
            if (existing)
            {
                ModelState.AddModelError("Title", "Bu başlıkla oluşturulmuş bir slayt zaten var.");
            }

            var conflictOrder = await _repository.Slides
                .AnyAsync(s => s.Order == model.Order && s.IsActive && model.IsActive);

            if (conflictOrder)
            {
                ModelState.AddModelError("Order", "Bu sırada zaten aktif bir slayt var. Lütfen farklı bir sıra numarası girin.");
            }

            if (ModelState.IsValid)
            {
                var ext = Path.GetExtension(model.ImageFile!.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg, .png veya .gif uzantılı görseller yüklenebilir.");
                    return View(model);
                }

                var imageName = $"{Guid.NewGuid()}{ext}";
                var path = Path.Combine(_env.WebRootPath, "img", "slides", imageName);

                using var stream = new FileStream(path, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);

                var slide = new Slide
                {
                    ImageUrl = imageName,
                    Title = model.Title,
                    Link = generatedSlug!,
                    Order = model.Order,
                    IsActive = model.IsActive
                };

                await _repository.AddSlideAsync(slide);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slide = await _repository.GetSlideByIdAsync(id);
            if (slide == null) return NotFound();

            var model = new SlideCreateViewModel
            {
                Title = slide.Title,
                Link = slide.Link,
                Order = slide.Order,
                IsActive = slide.IsActive
            };

            ViewBag.ExistingImage = slide.ImageUrl;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SlideCreateViewModel model)
        {
            var existingSlide = await _repository.GetSlideByIdAsync(id);
            if (existingSlide == null) return NotFound();

            var generatedSlug = "/" + model.Title?.ToUrlSlug();

            if (!string.IsNullOrEmpty(model.Link) && model.Link.ToUrlSlug() != generatedSlug)
            {
                ModelState.AddModelError("Link", "Link ile başlıktan üretilen URL uyuşmuyor.");
                model.Link = generatedSlug;
            }

            var duplicateSlug = await _repository.Slides
                .AnyAsync(s => s.Link == generatedSlug && s.Id != id);

            if (duplicateSlug)
            {
                ModelState.AddModelError("Title", "Bu başlıkla oluşturulmuş başka bir slayt zaten var.");
            }

            var conflictOrder = await _repository.Slides
                .AnyAsync(s => s.Order == model.Order && s.IsActive && model.IsActive && s.Id != id);

            if (conflictOrder)
            {
                ModelState.AddModelError("Order", "Bu sırada zaten aktif bir slayt var. Lütfen farklı bir sıra numarası girin.");
            }

            if (ModelState.IsValid)
            {
                string imageName = existingSlide.ImageUrl;

                if (model.ImageFile != null)
                {
                    var ext = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                    if (!allowedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg, .png veya .gif uzantılı görseller yüklenebilir.");
                        return View(model);
                    }

                    imageName = $"{Guid.NewGuid()}{ext}";
                    var path = Path.Combine(_env.WebRootPath, "img", "slides", imageName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await model.ImageFile.CopyToAsync(stream);
                }

                existingSlide.Title = model.Title;
                existingSlide.Link = generatedSlug!;
                existingSlide.Order = model.Order;
                existingSlide.IsActive = model.IsActive;
                existingSlide.ImageUrl = imageName;

                await _repository.UpdateSlideAsync(existingSlide);
                return RedirectToAction("Index");
            }

            ViewBag.ExistingImage = existingSlide.ImageUrl;
            return View(model);
        }
            
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var slide = await _repository.GetSlideByIdAsync(id);
            if (slide == null)
                return NotFound();

            var imagePath = Path.Combine(_env.WebRootPath, "img", "slides", slide.ImageUrl);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            await _repository.DeleteSlideAsync(slide);
            return RedirectToAction("Index");
        }


    }
}
