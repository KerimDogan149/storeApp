using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StoreApp.Data.Entities;
using StoreApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using StoreApp.Data.Helpers;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;
using StoreApp.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Entities;



namespace StoreApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly IStoreRepository _storeRepository;
        private readonly LocationService _locationService;



        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IStoreRepository storeRepository, LocationService locationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _storeRepository = storeRepository;
            _locationService = locationService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingEmailUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingEmailUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Bu e-posta adresi zaten kayıtlı.");
                return View(model);
            }

            var existingUserNameUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserNameUser != null)
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı zaten kullanımda.");
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }




        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerLoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, isPersistent: false, lockoutOnFailure: false); ;

            if (result.Succeeded)
            {
                return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Geçersiz giriş denemesi.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AccountInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewBag.Error = "Kullanıcı bulunamadı.";
                return View(new AccountInfoViewModel());
            }

            Console.WriteLine($"GET AccountInfo - UserId: {user.Id}");
            Console.WriteLine($"GET AccountInfo - FirstName: {user.FirstName}, LastName: {user.LastName}, Phone: {user.PhoneNumber}");

            var model = new AccountInfoViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AccountInfo(AccountInfoViewModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Model boş geldi.");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            var existingUserWithSameUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserWithSameUserName != null && existingUserWithSameUserName.Id != user.Id)
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı başka bir kullanıcı tarafından kullanılıyor.");
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;


            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Bilgileriniz başarıyla güncellendi.";
                return RedirectToAction("AccountInfo");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", $"{error.Code} - {error.Description}");
                }
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("AccountInfo");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }







        [Authorize]
        public async Task<IActionResult> Addresses()
        {
            var user = await _userManager.GetUserAsync(User);
            var addresses = await _storeRepository.GetAddressesByUserIdAsync(user.Id);
            return View(addresses);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddAddress()
        {
            var cities = await _locationService.GetAllCitiesAsync();
            var model = new AddressViewModel
            {
                Provinces = cities.Select(c => new SelectListItem
                {
                    Value = c.sehir_id,
                    Text = c.sehir_adi
                }).ToList()
            };

            return View(model);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            var address = new Address
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Province = model.Province,
                District = model.District,
                Neighborhood = model.Neighborhood,
                FullAddress = model.FullAddress,
                Title = model.Title,
                AppUserId = user.Id
            };

            await _storeRepository.AddAddressAsync(address);
            return RedirectToAction(nameof(Addresses));
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditAddress(int id)
        {
            var address = await _storeRepository.GetAddressByIdAsync(id);
            if (address == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (address.AppUserId != userId)
                return Forbid();

            var model = new AddressViewModel
            {
                Id = address.Id,
                Title = address.Title,
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                Province = address.Province,
                District = address.District,
                Neighborhood = address.Neighborhood,
                FullAddress = address.FullAddress
            };

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var cities = await _locationService.GetAllCitiesAsync();
                model.Provinces = cities.Select(c => new SelectListItem
                {
                    Value = c.sehir_adi,
                    Text = c.sehir_adi
                }).ToList();

                return View(model);
            }

            var address = await _storeRepository.GetAddressByIdAsync(model.Id.Value);
            if (address == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (address.AppUserId != userId)
                return Forbid();

            address.Title = model.Title;
            address.FullName = model.FullName;
            address.PhoneNumber = model.PhoneNumber;
            address.Province = model.Province;
            address.District = model.District;
            address.Neighborhood = model.Neighborhood;
            address.FullAddress = model.FullAddress;

            await _storeRepository.UpdateAddressAsync(address);

            TempData["success"] = "Adres başarıyla güncellendi.";
            return RedirectToAction(nameof(Addresses));
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _storeRepository.GetAddressByIdAsync(id);
            if (address == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (address.AppUserId != userId)
                return Forbid();

            await _storeRepository.DeleteAddressAsync(address);
            TempData["success"] = "Adres başarıyla silindi.";
            return RedirectToAction(nameof(Addresses));
        }


        //AdresApi
        [HttpGet]
        public async Task<JsonResult> GetAllProvinces()
        {
            var cities = await _locationService.GetAllCitiesAsync();
            var provinces = cities
                .Select(c => c.sehir_adi)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return Json(provinces);
        }


        [HttpGet]
        public async Task<JsonResult> GetDistricts(string province)
        {
            var cities = await _locationService.GetAllCitiesAsync();
            var matchedCity = cities.FirstOrDefault(c => c.sehir_adi == province);
            if (matchedCity == null)
                return Json(new List<string>());

            var districts = await _locationService.GetDistrictsByProvinceIdAsync(matchedCity.sehir_id);
            return Json(districts
                .Select(d => d.ilce_adi)
                .Distinct()
                .OrderBy(d => d)
                .ToList());
        }


        [HttpGet]
        public async Task<JsonResult> GetNeighborhoods(string province, string district)
        {
            var cities = await _locationService.GetAllCitiesAsync();
            var matchedCity = cities.FirstOrDefault(c => c.sehir_adi == province);
            if (matchedCity == null)
                return Json(new List<string>());

            var districts = await _locationService.GetDistrictsByProvinceIdAsync(matchedCity.sehir_id);
            var matchedDistrict = districts.FirstOrDefault(d => d.ilce_adi == district);
            if (matchedDistrict == null)
                return Json(new List<string>());

            var neighborhoods = await _locationService.GetNeighborhoodsByDistrictIdAsync(matchedDistrict.ilce_id);
            return Json(neighborhoods
                .Select(n => n.mahalle_adi)
                .Distinct()
                .OrderBy(n => n)
                .ToList());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Orders(UserOrdersFilterViewModel filter)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (filter.StartDate.HasValue && filter.EndDate.HasValue)
            {
                if (filter.StartDate > filter.EndDate)
                {
                    ModelState.AddModelError("", "Başlangıç tarihi, bitiş tarihinden sonra olamaz.");
                }
            }
            if (filter.MinTotal.HasValue && filter.MaxTotal.HasValue)
            {
                if (filter.MinTotal > filter.MaxTotal)
                {
                    ModelState.AddModelError("", "Minimum tutar, maksimum tutardan büyük olamaz.");
                }
            }


            var query = _storeRepository.Orders
                .Where(o => o.AppUserId == user.Id)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductCategories)
                .Include(o => o.Address)
                .AsQueryable();


            if (ModelState.IsValid)
            {
                if (filter.Status.HasValue)
                {
                    query = query.Where(o => o.Status == filter.Status.Value);
                }

                if (filter.StartDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate >= filter.StartDate.Value);
                }

                if (filter.EndDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate <= filter.EndDate.Value);
                }

                if (filter.MinTotal.HasValue)
                {
                    query = query.Where(o => o.TotalAmount >= filter.MinTotal.Value);
                }

                if (filter.MaxTotal.HasValue)
                {
                    query = query.Where(o => o.TotalAmount <= filter.MaxTotal.Value);
                }

                if (!string.IsNullOrEmpty(filter.ProductName))
                {
                    var lowered = filter.ProductName.ToLower();
                    query = query.Where(o =>
                        o.OrderItems.Any(oi => oi.Product.Name.ToLower().Contains(lowered)));
                }

                if (filter.CategoryId.HasValue)
                {
                    query = query.Where(o =>
                        o.OrderItems.Any(oi =>
                            oi.Product.ProductCategories.Any(pc => pc.CategoryId == filter.CategoryId)));
                }
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var model = orders
                .Where(o => o.Address != null)
                .Select(o => new OrderSummaryViewModel
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Address = o.Address,
                    Items = o.OrderItems,
                    Status = o.Status
                }).ToList();

            ViewBag.Filter = filter;
            ViewBag.Categories = await _storeRepository.Categories.ToListAsync();

            return View(model);
        }

    }
}
