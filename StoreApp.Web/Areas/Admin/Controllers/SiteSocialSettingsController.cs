using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
using StoreApp.Data.Entities;
using StoreApp.Web.Areas.Admin.Models;

namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SiteSocialSettingsController : Controller
    {
        private readonly IStoreRepository _repository;

        public SiteSocialSettingsController(IStoreRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> SiteSocialSettings()
        {
            var setting = await _repository.GetSiteSocialSettingsAsync();

            var model = new SiteSocialSettingsViewModel
            {
                Id = setting?.Id ?? 0,
                PhoneNumber = setting?.PhoneNumber,
                FacebookUrl = setting?.FacebookUrl,
                InstagramUrl = setting?.InstagramUrl,
                TwitterUrl = setting?.TwitterUrl,
                YoutubeUrl = setting?.YoutubeUrl
            };

            return View("SiteSocialSettings", model);
        }
    
    [HttpPost]
    public async Task<IActionResult> SiteSocialSettings(SiteSocialSettingsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("SiteSocialSettings", model);
        }

        var entity = new SiteSocialAddressSetting
        {
            Id = model.Id,
            PhoneNumber = model.PhoneNumber,
            FacebookUrl = model.FacebookUrl,
            InstagramUrl = model.InstagramUrl,
            TwitterUrl = model.TwitterUrl,
            YoutubeUrl = model.YoutubeUrl
        };

        await _repository.UpdateSiteSocialSettingsAsync(entity);

        TempData["success"] = "Sosyal medya ayarları başarıyla güncellendi.";
        return RedirectToAction(nameof(SiteSocialSettings));
    }
}

}
