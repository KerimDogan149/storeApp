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


        [HttpGet]
        public IActionResult ProductSalesList(string name = null, int? categoryId = null, string sortOrder = "desc", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                ModelState.AddModelError("", "Başlangıç tarihi, bitiş tarihinden sonra olamaz.Filtreleri temizleyip tekrardan deneyiniz.");
            }

            var productsQuery = _repository.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(name.ToLower()));

            if (categoryId.HasValue)
                productsQuery = productsQuery.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId.Value));

            var productList = productsQuery.ToList();

            var deliveredOrderItems = _repository.OrderItems
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.Status == OrderStatus.TeslimEdildi)
                .ToList();

            if (startDate.HasValue)
                deliveredOrderItems = deliveredOrderItems.Where(oi => oi.Order.OrderDate >= startDate.Value).ToList();

            if (endDate.HasValue)
                deliveredOrderItems = deliveredOrderItems.Where(oi => oi.Order.OrderDate <= endDate.Value).ToList();

            var productSalesData = productList.Select(p =>
            {
                var soldItems = deliveredOrderItems.Where(oi => oi.ProductId == p.Id);

                return new ProductSalesListItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Image = p.Image,
                    Categories = string.Join(", ", p.ProductCategories.Select(pc => pc.Category.Name)),
                    TotalQuantitySold = soldItems.Sum(oi => oi.Quantity)
                };
            });

            if (sortOrder == "asc")
                productSalesData = productSalesData.OrderBy(p => p.TotalQuantitySold);
            else
                productSalesData = productSalesData.OrderByDescending(p => p.TotalQuantitySold);

            var categories = _repository.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            var viewModel = new ProductSalesListViewModel
            {
                Products = productSalesData.ToList(),
                Categories = categories,
                FilterName = name,
                FilterCategoryId = categoryId,
                SortOrder = sortOrder,
                StartDate = startDate,
                EndDate = endDate
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ProductSalesDetails(int id)
        {
            var product = _repository.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            var sales = _repository.OrderItems
                .Where(oi => oi.ProductId == id && oi.Order.Status == OrderStatus.TeslimEdildi)
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.AppUser)
                .Select(oi => new ProductSaleDetailItemViewModel
                {

                    Email = oi.Order.AppUser.Email,
                    OrderId = oi.Order.Id,
                    UserName = oi.Order.AppUser.UserName,
                    OrderDate = oi.Order.OrderDate,
                    Quantity = oi.Quantity
                })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            var viewModel = new ProductSalesDetailsViewModel
            {
                ProductId = id,
                ProductName = product.Name,
                Categories = string.Join(", ", product.ProductCategories.Select(pc => pc.Category.Name)),
                Image = product.Image,


                Sales = sales
            };

            return View(viewModel);
        }


    }
}