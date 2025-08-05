using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
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

namespace StoreApp.Web.Controllers;

public class HomeController : Controller
{
    public int pageSize = 4;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
            private readonly UserManager<AppUser> _userManager;

    public HomeController(IStoreRepository storeRepository, IMapper mapper, UserManager<AppUser> userManager)
    {
        _storeRepository = storeRepository;
        _mapper = mapper;
        _userManager = userManager;

    }
    public IActionResult Index(string category, int page = 1)
    {
        var featuredProducts = _storeRepository
                .GetFeaturedProducts()
                .Where(p => p.IsApproved)
                .Select(p => _mapper.Map<ProductViewModel>(p));

        var bestSellers = _storeRepository.GetBestSellerProducts()
        .Where(p => p.IsApproved)
        .Select(p => _mapper.Map<ProductViewModel>(p))
        .ToList();

        var slides = _storeRepository
        .GetActiveSlides()
        .OrderBy(s => s.Order)
        .ToList();

        var campaigns = _storeRepository
        .GetAllCampaignsAsync();

          List<int> favoriteProductIds = new();
        if (User.Identity!.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(User);
            favoriteProductIds = _storeRepository
                .Favorites
                .Where(f => f.AppUserId == userId)
                .Select(f => f.ProductId)
                .ToList();
        }
        return View(new ProductListViewModel
        {
            Products = _storeRepository.GetProductsByCategory(category, page, pageSize)
                .Where(p => p.IsApproved)
             .Select(product => _mapper.Map<ProductViewModel>(product)),
            FeaturedProducts = featuredProducts,
            BestSellerProducts = bestSellers,
            Slides = slides,
            Campaigns = campaigns.Result,
            FavoriteProductIds = favoriteProductIds, // ✅ Favoriler burada ekleniyor
            PageInfo = new PageInfo
            {
                ItemsPerPage = pageSize,
                CurrentPage = page,
                TotalItems = _storeRepository.GetProductCount(category)

            }
        });

    }


[HttpGet]
public async Task<IActionResult> Details(string url)
{
    if (string.IsNullOrWhiteSpace(url))
        return NotFound();

    var product = await _storeRepository.Products
        .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
        .FirstOrDefaultAsync(p => p.Url == url && p.IsApproved);

    if (product == null)
        return NotFound();

    // Favori kontrolü (kullanıcı giriş yapmışsa)
    bool isFavorited = false;
    if (User.Identity.IsAuthenticated)
    {
        var userId = _userManager.GetUserId(User);
        isFavorited = await _storeRepository.IsProductFavoritedAsync(userId, product.Id);
    }

    var viewModel = new ProductViewModel
    {
        Id = product.Id,
        Name = product.Name,
        Url = product.Url,
        Description = product.Description,
        Image = product.Image,
        Price = product.Price,
        Stock = product.Stock,
        IsFeatured = product.IsFeatured,
        IsBestSeller = product.IsBestSeller,
        ProductCategories = product.ProductCategories.ToList(),
        IsFavorited = isFavorited
    };

    return View(viewModel);
}



public async Task<IActionResult> Category(string category, int page = 1)
{
    int pageSize = 12;

    var filteredProducts = _storeRepository.Products
        .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
        .Where(p => p.IsApproved && p.ProductCategories.Any(pc => pc.Category.Url.ToLower() == category.ToLower()))
        .ToList();

    var productViewModels = filteredProducts
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Url = p.Url,
            Description = p.Description,
            Image = p.Image,
            Price = p.Price,
            Category = p.ProductCategories.FirstOrDefault()?.Category?.Name ?? ""
        })
        .ToList();

    int totalItems = filteredProducts.Count;

    var categoryName = _storeRepository.Categories
        .FirstOrDefault(c => c.Url.ToLower() == category.ToLower())?.Name;

    ViewBag.CategoryName = categoryName ?? "Kategori";

    var user = await _userManager.GetUserAsync(User);
    var userId = user?.Id;

    var favoriteIds = userId != null
        ? await _storeRepository.GetFavoriteProductIdsAsync(userId)
        : new List<int>();

    var viewModel = new ProductListViewModel
    {
        Products = productViewModels,
        PageInfo = new PageInfo
        {
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalItems = totalItems
        },
        FavoriteProductIds = favoriteIds
    };

    return View(viewModel);
}


    public IActionResult Search(string q)
    {
        q = q ?? "";

        if (string.IsNullOrWhiteSpace(q))
            return RedirectToAction("Index");

        var matchedProducts = _storeRepository.Products
            .Where(p => p.IsApproved && p.Name.ToLower().Contains(q.ToLower()))
            .ToList();

        var products = matchedProducts
            .Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Url = p.Url,
                Description = p.Description,
                Price = p.Price,
                Image = p.Image,
                Category = p.ProductCategories.FirstOrDefault()?.Category?.Name ?? ""
            })
            .ToList();


        var viewModel = new ProductListViewModel
        {
            Products = products
        };

        ViewBag.SearchTerm = q;
        return View("Search", viewModel);
    }

    [Route("campaign/{url}")]
    public async Task<IActionResult> CampaignDetail(string url)
    {
        if (string.IsNullOrEmpty(url))
            return NotFound();

        var campaign = await _storeRepository.GetCampaignByUrlAsync(url);

        if (campaign == null)
            return NotFound();

        return View("CampaignDetail", campaign);
    }



}