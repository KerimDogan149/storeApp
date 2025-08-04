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
using StoreApp.Web.Areas.Admin.Models.Reports;

namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]


    [Area("Admin")]

    public class ReportController : Controller
    {
        private readonly IStoreRepository _repository;

        public ReportController(IStoreRepository repository)
        {
            _repository = repository;
        }

public IActionResult Dashboard(int? year = null, int? month = null, string chartType = "product")
{
    try
    {
        var ordersQuery = _repository.Orders
            .Where(o => o.Status == OrderStatus.TeslimEdildi)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
            .AsQueryable();

        if (year.HasValue)
        {
            ordersQuery = ordersQuery.Where(o => o.OrderDate.Year == year.Value);
        }

        if (month.HasValue)
        {
            ordersQuery = ordersQuery.Where(o => o.OrderDate.Month == month.Value);
        }

        var orders = ordersQuery.ToList();

        var totalRevenue = orders.Sum(o => o.TotalAmount);

        List<string> chartLabels = new();
        List<decimal> chartValues = new();

        if (chartType == "product")
        {
            var productGroups = orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(10)
                .ToList();

            chartLabels = productGroups.Select(p => p.ProductName).ToList();
            chartValues = productGroups.Select(p => p.Revenue).ToList();
        }
        else if (chartType == "category")
        {
            var categoryGroups = orders
                .SelectMany(o => o.OrderItems)
                .SelectMany(oi => oi.Product.ProductCategories.Select(pc => new
                {
                    CategoryName = pc.Category.Name,
                    Revenue = oi.Price * oi.Quantity
                }))
                .GroupBy(x => x.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    Revenue = g.Sum(x => x.Revenue)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(10)
                .ToList();

            chartLabels = categoryGroups.Select(c => c.CategoryName).ToList();
            chartValues = categoryGroups.Select(c => c.Revenue).ToList();
        }
        else if (chartType == "month")
        {

            var monthlyGroups = orders
                .GroupBy(o => o.OrderDate.ToString("yyyy-MM"))
                .Select(g => new
                {
                    Month = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            chartLabels = monthlyGroups.Select(m => m.Month).ToList();
            chartValues = monthlyGroups.Select(m => m.Revenue).ToList();
        }
        else
        {
            // Default product bazlı göster
            var productGroups = orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(10)
                .ToList();

            chartLabels = productGroups.Select(p => p.ProductName).ToList();
            chartValues = productGroups.Select(p => p.Revenue).ToList();
        }

        var topSellers = orders
            .SelectMany(o => o.OrderItems)
            .GroupBy(oi => oi.Product.Name)
            .Select(g => new TopSellingProductViewModel
            {
                ProductName = g.Key,
                QuantitySold = g.Sum(oi => oi.Quantity),
                Url = g.First().Product.Url
            })
            .OrderByDescending(p => p.QuantitySold)
            .Take(10)
            .ToList();

        var viewModel = new ReportDashboardViewModel
        {
            MonthlySales = orders
                .GroupBy(o => o.OrderDate.ToString("yyyy-MM"))
                .Select(g => new MonthlySalesViewModel
                {
                    Month = g.Key,
                    OrderCount = g.Count(),
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(x => x.Month)
                .ToList(),

            TopSellers = topSellers,
            TotalRevenue = totalRevenue,
            SelectedYear = year,
            SelectedMonth = month,
            ChartLabels = chartLabels,
            ChartValues = chartValues,
            ChartType = chartType
        };

        return View(viewModel);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Error in Dashboard action: {ex.Message}");
        Debug.WriteLine(ex.StackTrace);
        return View("Error");
    }
}


    }
}