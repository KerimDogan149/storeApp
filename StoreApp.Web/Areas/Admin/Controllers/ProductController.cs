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
using OfficeOpenXml;
using StoreApp.Web.Areas.Admin.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;


namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]


    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IStoreRepository _repository;

        public ProductController(IStoreRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(int? categoryId, string search, string isFeatured, string isBestSeller)
        {
            var products = _repository.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId.Value));
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
                    ProductCategories = new List<ProductCategory>()
                };

                foreach (var catId in model.SelectedCategoryIds)
                {
                    product.ProductCategories.Add(new ProductCategory
                    {
                        Product = product,
                        CategoryId = catId
                    });
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
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
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
                SelectedCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList(),
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
                    .Include(p => p.ProductCategories)
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

                await _repository.RemoveProductCategoriesAsync(product.ProductCategories);

                product.ProductCategories = model.SelectedCategoryIds
                    .Select(catId => new ProductCategory
                    {
                        ProductId = product.Id,
                        CategoryId = catId
                    }).ToList();

                await _repository.UpdateProductAsync(product);
                return RedirectToAction("Index");
            }

            model.AllCategories = _repository.Categories.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _repository.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefault(p => p.Id == id);

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

            await _repository.RemoveProductCategoriesAsync(product.ProductCategories);

            await _repository.DeleteProductAsync(product.Id);

            return RedirectToAction("Index");
        }

        // 1. Yeni Ürün Ekleme
        [HttpGet]
        public IActionResult DownloadNewTemplate()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Urunler");
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Price";
                worksheet.Cells[1, 3].Value = "Stock";
                worksheet.Cells[1, 4].Value = "CategoryNames";

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "YeniUrunSablonu.xlsx"
                );
            }
        }
        [HttpGet]
        public IActionResult DownloadUpdateTemplate()
        {
            var products = _repository.Products
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    p.Stock,
                    Categories = string.Join(", ", p.ProductCategories.Select(pc => pc.Category.Name))
                }).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Ürünler");

                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Price";
                worksheet.Cells[1, 3].Value = "Stock";
                worksheet.Cells[1, 4].Value = "CategoryNames";

                int row = 2;
                foreach (var p in products)
                {
                    worksheet.Cells[row, 1].Value = p.Name;
                    worksheet.Cells[row, 2].Value = p.Price;
                    worksheet.Cells[row, 3].Value = p.Stock;
                    worksheet.Cells[row, 4].Value = p.Categories;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "UrunGuncellemeSablonu.xlsx");
            }
        }



        [HttpGet]
        public IActionResult NewBulkUpload()
        {
            return View(new ProductBulkUploadViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> NewBulkUpload(ProductBulkUploadViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var errors = new List<string>();
            var newProducts = new List<Product>();

            if (model.File != null && model.File.Length > 0)
            {
                var ext = Path.GetExtension(model.File.FileName).ToLower();
                if (ext != ".xlsx")
                {
                    ModelState.AddModelError("", "Sadece .xlsx dosya formatı destekleniyor.");
                    return View(model);
                }

                using (var stream = new MemoryStream())
                {
                    await model.File.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet == null || worksheet.Dimension == null || worksheet.Dimension.Rows < 2)
                        {
                            errors.Add("Excel dosyası boş veya geçersiz. En az bir veri satırı içermelidir.");
                        }
                        else
                        {
                            int rowCount = worksheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++)
                            {
                                string name = worksheet.Cells[row, 1].Text?.Trim();
                                bool validPrice = decimal.TryParse(worksheet.Cells[row, 2].Text, out decimal price);
                                bool validStock = int.TryParse(worksheet.Cells[row, 3].Text, out int stock);
                                string categoryString = worksheet.Cells[row, 4].Text?.Trim();

                                if (string.IsNullOrWhiteSpace(name))
                                {
                                    errors.Add($"Satır {row}, Sütun 1: Ürün adı boş olamaz.");
                                    continue;
                                }

                                if (!validPrice)
                                {
                                    errors.Add($"Satır {row}, Sütun 2: Geçersiz fiyat değeri.");
                                    continue;
                                }

                                if (!validStock)
                                {
                                    errors.Add($"Satır {row}, Sütun 3: Geçersiz stok değeri.");
                                    continue;
                                }

                                if (string.IsNullOrWhiteSpace(categoryString))
                                {
                                    errors.Add($"Satır {row}, Sütun 4: Kategori boş olamaz.");
                                    continue;
                                }

                                string url = name.ToUrlSlug();
                                var exists = _repository.Products.Any(p => p.Url == url);
                                if (exists)
                                {
                                    errors.Add($"Satır {row}: '{name}' isimli ürün sistemde zaten mevcut.");
                                    continue;
                                }

                                var categoryNames = categoryString
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(c => c.Trim().ToLower())
                                    .ToList();

                                var matchedCategories = _repository.Categories
                                    .Where(c => categoryNames.Contains(c.Name.ToLower()))
                                    .ToList();

                                if (matchedCategories.Count != categoryNames.Count)
                                {
                                    var missing = categoryNames
                                        .Except(matchedCategories.Select(c => c.Name.ToLower()));
                                    errors.Add($"Satır {row}: Aşağıdaki kategoriler sistemde bulunamadı: {string.Join(", ", missing)}");
                                    continue;
                                }

                                var productCategories = matchedCategories.Select(cat => new ProductCategory
                                {
                                    Category = cat
                                }).ToList();

                                newProducts.Add(new Product
                                {
                                    Name = name,
                                    Url = url,
                                    Price = price,
                                    Stock = stock,
                                    IsApproved = false,
                                    CreatedDate = DateTime.Now,
                                    ProductCategories = productCategories
                                });
                            }
                        }
                    }
                }


                if (errors.Any())
                {
                    ViewBag.Errors = errors;
                    return View(model);
                }

                if (!newProducts.Any())
                {
                    ViewBag.Errors = new List<string> { "Geçerli ürün bulunamadı. Dosyanızı kontrol edin." };
                    return View(model);
                }

                foreach (var product in newProducts)
                {
                    await _repository.CreateProductAsync(product);
                }

                TempData["success"] = $"{newProducts.Count} ürün başarıyla eklendi (onay bekliyor).";
                return RedirectToAction("NewBulkUpload");
            }

            ModelState.AddModelError("", "Dosya yüklenemedi.");
            return View(model);
        }



        // 2. Ürün Güncelleme
        [HttpGet]
        public IActionResult UpdateBulkUpload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBulkUpload(ProductBulkUploadViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var errors = new List<string>();
            var updatedCount = 0;

            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("", "Dosya yüklenemedi.");
                return View(model);
            }

            var ext = Path.GetExtension(model.File.FileName).ToLower();
            if (ext != ".xlsx")
            {
                ModelState.AddModelError("", "Sadece .xlsx dosya formatı destekleniyor.");
                return View(model);
            }

            using (var stream = new MemoryStream())
            {
                await model.File.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null || worksheet.Dimension == null || worksheet.Dimension.Rows < 2)
                    {
                        errors.Add("Excel dosyası boş veya geçersiz.");
                    }
                    else
                    {
                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            string name = worksheet.Cells[row, 1].Text?.Trim();
                            bool validPrice = decimal.TryParse(worksheet.Cells[row, 2].Text, out decimal price);
                            bool validStock = int.TryParse(worksheet.Cells[row, 3].Text, out int stock);
                            string categoryString = worksheet.Cells[row, 4].Text?.Trim();

                            if (string.IsNullOrWhiteSpace(name))
                            {
                                errors.Add($"Satır {row}: Ürün adı boş olamaz.");
                                continue;
                            }

                            if (!validPrice)
                            {
                                errors.Add($"Satır {row}: Geçersiz fiyat değeri.");
                                continue;
                            }

                            if (!validStock)
                            {
                                errors.Add($"Satır {row}: Geçersiz stok değeri.");
                                continue;
                            }

                            if (string.IsNullOrWhiteSpace(categoryString))
                            {
                                errors.Add($"Satır {row}: Kategori alanı boş olamaz.");
                                continue;
                            }

                            var product = _repository.Products
                                .Include(p => p.ProductCategories)
                                .FirstOrDefault(p => p.Name.ToLower() == name.ToLower());

                            if (product == null)
                            {
                                errors.Add($"Satır {row}: '{name}' isminde bir ürün bulunamadı.");
                                continue;
                            }

                            var categoryNames = categoryString
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(c => c.Trim().ToLower())
                                .Distinct()
                                .ToList();

                            var matchedCategories = _repository.Categories
                                .Where(c => categoryNames.Contains(c.Name.ToLower()))
                                .ToList();

                            if (matchedCategories.Count != categoryNames.Count)
                            {
                                var missing = categoryNames.Except(matchedCategories.Select(c => c.Name.ToLower()));
                                errors.Add($"Satır {row}: Sistem dışı kategori(ler): {string.Join(", ", missing)}");
                                continue;
                            }

                            product.Price = price;
                            product.Stock = stock;
                            product.IsApproved = false;

                            product.ProductCategories.Clear();
                            foreach (var cat in matchedCategories)
                            {
                                product.ProductCategories.Add(new ProductCategory
                                {
                                    ProductId = product.Id,
                                    CategoryId = cat.Id
                                });
                            }

                            updatedCount++;
                        }
                    }
                }
            }

            if (errors.Any())
            {
                ViewBag.Errors = errors;
                return View(model);
            }

            await _repository.SaveChangesAsync();

            TempData["success"] = $"{updatedCount} ürün başarıyla güncellendi.";
            return RedirectToAction("UpdateBulkUpload");
        }


        [HttpGet]
        public IActionResult PendingApproval(ProductApprovalFilterViewModel filter)
        {
            var query = _repository.Products
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .Where(p => !p.IsApproved);

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == filter.CategoryId));
            }

            var products = query.ToList();

            // Kategori listesi dropdown için
            var categories = _repository.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            filter.Products = products;
            filter.Categories = categories;

            return View(filter);
        }
                [HttpPost]
        public async Task<IActionResult> ApproveSelected(List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
            {
                TempData["error"] = "Herhangi bir ürün seçilmedi.";
                return RedirectToAction("PendingApproval");
            }

            var productsToApprove = _repository.Products
                .Where(p => productIds.Contains(p.Id) && !p.IsApproved)
                .ToList();

            foreach (var product in productsToApprove)
            {
                product.IsApproved = true;
            }

            await _repository.SaveChangesAsync();

            TempData["success"] = $"{productsToApprove.Count} ürün onaylandı.";
            return RedirectToAction("PendingApproval");
        }




    }

}