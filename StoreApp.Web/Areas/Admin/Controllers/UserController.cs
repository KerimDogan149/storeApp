using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Entities;
using StoreApp.Web.Areas.Admin.Models;
using StoreApp.Data.Concrete;


namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index(string username, string email)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                users = users.Where(u => u.UserName.Contains(username));
            }

            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(u => u.Email.Contains(email));
            }

            var userList = users
                .OrderBy(u => u.UserName)
                .ToList();

            return View(userList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Email kontrolü
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Bu email adresi zaten kullanımda.");
                return View(model);
            }

            // Kullanıcı adı kontrolü
            var existingUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserName != null)
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı zaten kullanımda.");
                return View(model);
            }


            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var model = new UserEditViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            // Kullanıcı adı başka biri tarafından kullanılıyor mu?
            var userNameOwner = await _userManager.FindByNameAsync(model.UserName);
            if (userNameOwner != null && userNameOwner.Id != user.Id)
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı zaten kullanımda.");
                return View(model);
            }

            // Email başka biri tarafından kullanılıyor mu?
            var emailOwner = await _userManager.FindByEmailAsync(model.Email);
            if (emailOwner != null && emailOwner.Id != user.Id)
            {
                ModelState.AddModelError(nameof(model.Email), "Bu email adresi zaten kullanımda.");
                return View(model);
            }

            // Bilgileri güncelle
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            // Şifre güncelleme isteği varsa
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!passResult.Succeeded)
                {
                    foreach (var error in passResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("Index");
        }


    }
}