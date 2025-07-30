using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
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

namespace StoreApp.Web.Components
{
    public class SiteSocialViewComponent : ViewComponent
    {
        private readonly IStoreRepository _storeRepository;

        public SiteSocialViewComponent(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await _storeRepository.GetSiteSocialSettingsAsync();
            return View(setting);
        }
    }

}
