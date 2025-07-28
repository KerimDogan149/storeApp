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
    [Area("Admin")]
    [Authorize (Roles = "Admin,Manager")]

    public class CampaignController : Controller
    {

        private readonly IStoreRepository _repository;
        private readonly IWebHostEnvironment _env;

            public CampaignController(IStoreRepository repository, IWebHostEnvironment env)
            {
                _repository = repository;
                _env = env;
            }

        public async Task<IActionResult> Index(string status)
        {
            var campaigns = await _repository.GetAllCampaignsAsync();

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "active")
                {
                    campaigns = campaigns.Where(c => c.IsActive).ToList();
                }
                else if (status == "inactive")
                {
                    campaigns = campaigns.Where(c => !c.IsActive).ToList();
                }
            }
            else
            {
                campaigns = campaigns
                    .OrderByDescending(c => c.IsActive)
                    .ThenBy(c => c.Title)
                    .ToList();
            }

            return View(campaigns);
        }


            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(CampaignViewModel model)
            {

                if (model.ImageFile == null)
                {
                    ModelState.AddModelError("ImageFile", "Görsel dosyası yüklenmelidir.");
                }
                else
                {
                    var ext = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                    if (!allowedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg, .png veya .gif uzantılı görseller yüklenebilir.");
                    }
                }


                var generatedSlug = model.Title?.ToUrlSlug() ?? "";
                if (!string.IsNullOrEmpty(model.Url) && model.Url != generatedSlug)
                {
                    ModelState.AddModelError("Url", "URL ile başlıktan üretilen slug uyuşmuyor.");
                    model.Url = generatedSlug;
                }


                var isDuplicate = await _repository.Campaigns.AnyAsync(c => c.Url == generatedSlug);
                if (isDuplicate)
                {
                    ModelState.AddModelError("Title", "Bu başlıktan oluşturulmuş bir kampanya zaten var.");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string fileName = "";
                if (model.ImageFile != null)
                {
                    var ext = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                    fileName = $"{Guid.NewGuid()}{ext}";
                    var savePath = Path.Combine(_env.WebRootPath, "img", "campain", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await model.ImageFile.CopyToAsync(stream);
                }


                var campaign = new Campaign
                {
                    Title = model.Title,
                    SubTitle = model.SubTitle,
                    Url = generatedSlug,
                    Description = model.Description,
                    Link = model.Link,
                    IsActive = model.IsActive,
                    Image = fileName
                };

                await _repository.AddCampaignAsync(campaign);
                return RedirectToAction("Index");
            }

            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                var campaign = await _repository.GetCampaignByIdAsync(id);
                if (campaign == null)
                    return NotFound();

                var model = new CampaignViewModel
                {
                    Title = campaign.Title,
                    SubTitle = campaign.SubTitle,
                    Url = campaign.Url,
                    Description = campaign.Description,
                    Link = campaign.Link,
                    IsActive = campaign.IsActive,
                    ImageFile = null
                };

                ViewBag.CurrentImage = campaign.Image;
                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, CampaignViewModel model)
            {
                var campaign = await _repository.GetCampaignByIdAsync(id);
                if (campaign == null)
                    return NotFound();

                // Görsel uzantı kontrolü (isteğe bağlı olarak)
                if (model.ImageFile != null)
                {
                    var ext = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                    if (!allowedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg, .png veya .gif uzantılı görseller yüklenebilir.");
                    }
                }

                // Slug üretimi
                var generatedSlug = model.Title?.ToUrlSlug() ?? "";
                if (!string.IsNullOrEmpty(model.Url) && model.Url != generatedSlug)
                {
                    ModelState.AddModelError("Url", "URL ile başlıktan üretilen slug uyuşmuyor.");
                    model.Url = generatedSlug;
                }

                // Aynı URL varsa (kendisi hariç)
                var duplicate = await _repository.Campaigns
                    .AnyAsync(c => c.Url == generatedSlug && c.Id != id);
                if (duplicate)
                {
                    ModelState.AddModelError("Title", "Bu başlıktan oluşturulmuş bir başka kampanya zaten var.");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.CurrentImage = campaign.Image;
                    return View(model);
                }

                // Yeni görsel yüklendiyse, eskiyi değiştir
                if (model.ImageFile != null)
                {
                    var ext = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                    var imageName = $"{Guid.NewGuid()}{ext}";
                    var imagePath = Path.Combine(_env.WebRootPath, "img", "campain", imageName);

                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
                    using var stream = new FileStream(imagePath, FileMode.Create);
                    await model.ImageFile.CopyToAsync(stream);

                    campaign.Image = imageName;
                }

                // Güncelleme
                campaign.Title = model.Title;
                campaign.SubTitle = model.SubTitle;
                campaign.Url = generatedSlug;
                campaign.Description = model.Description;
                campaign.Link = model.Link;
                campaign.IsActive = model.IsActive;

                await _repository.UpdateCampaignAsync(campaign);
                return RedirectToAction("Index");
            }


            [HttpPost]
            public async Task<IActionResult> Delete(int id)
            {
                var campaign = await _repository.GetCampaignByIdAsync(id);
                if (campaign == null)
                    return NotFound();

                // Görsel varsa sil
                if (!string.IsNullOrEmpty(campaign.Image))
                {
                    var imagePath = Path.Combine(_env.WebRootPath, "img", "campain", campaign.Image);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                await _repository.DeleteCampaignAsync(campaign);
                return RedirectToAction("Index");
            }

    }
}