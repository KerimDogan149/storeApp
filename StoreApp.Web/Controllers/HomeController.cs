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

namespace StoreApp.Web.Controllers;

public class HomeController : Controller
{
    public int pageSize = 4;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    public HomeController(IStoreRepository storeRepository, IMapper mapper)
    {
        _storeRepository = storeRepository;
        _mapper = mapper;
    }
    public IActionResult Index(string category, int page = 1)
    {
        var featuredProducts = _storeRepository
                .GetFeaturedProducts()
                .Select(p => _mapper.Map<ProductViewModel>(p));

        var bestSellers = _storeRepository.GetBestSellerProducts()
        .Select(p => _mapper.Map<ProductViewModel>(p))
        .ToList();

        var slides = _storeRepository
        .GetActiveSlides()
        .OrderBy(s => s.Order)
        .ToList();

        var campaigns = _storeRepository
        .GetAllCampaignsAsync();


        return View(new ProductListViewModel
        {
            Products = _storeRepository.GetProductsByCategory(category, page, pageSize)
             .Select(product => _mapper.Map<ProductViewModel>(product)),
            FeaturedProducts = featuredProducts,
            BestSellerProducts = bestSellers,
            Slides = slides,
            Campaigns = campaigns.Result,
            PageInfo = new PageInfo
            {
                ItemsPerPage = pageSize,
                CurrentPage = page,
                TotalItems = _storeRepository.GetProductCount(category)

            }
        });

    }


    public IActionResult Details(string url)
    {
        if (string.IsNullOrEmpty(url))
            return NotFound();
        var product = _storeRepository.Products
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .FirstOrDefault(p => p.Url == url);


        if (product == null)
            return NotFound();

        return View(product);
    }



    public IActionResult Category(string category, int page = 1)
    {
        int pageSize = 12;

        var filteredProducts = _storeRepository.Products
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .Where(p => p.ProductCategories.Any(pc => pc.Category.Url.ToLower() == category.ToLower()))
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

        var viewModel = new ProductListViewModel
        {
            Products = productViewModels,
            PageInfo = new PageInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            }
        };

        return View(viewModel);
    }

    public IActionResult Search(string q)
    {
        q = q ?? "";

        if (string.IsNullOrWhiteSpace(q))
            return RedirectToAction("Index");

        var matchedProducts = _storeRepository.Products
            .Where(p => p.Name.ToLower().Contains(q.ToLower()))
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