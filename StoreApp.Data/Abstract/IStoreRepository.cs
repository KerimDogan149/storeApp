using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;
using StoreApp.Data.Entities;

namespace StoreApp.Data.Abstract
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }

        IQueryable<Category> Categories { get; }

        IQueryable<Slide> Slides { get; }

        IQueryable<Campaign> Campaigns { get; }

        IQueryable<Address> Addresses { get; }

        IQueryable<Order> Orders { get; }
        IQueryable<OrderItem> OrderItems { get; }

        // PRODUCT VE CATEGORY İŞLEMLERİ
        void CreateProduct(Product entity);

        Task CreateCategoryAsync(Category entity);

        Task CreateProductAsync(Product entity);

        Task UpdateProductAsync(Product entity);

        Task UpdateCategoryAsync(Category entity);

        Task DeleteCategoryAsync(int categoryId);

        Task DeleteProductAsync(int productId);
        int GetProductCount(string category);

        IEnumerable<Product> GetProductsByCategory(string category, int page, int pageSize);

        IEnumerable<Product> GetFeaturedProducts();
        IEnumerable<Product> GetBestSellerProducts();

        // SLIDE İŞLEMLERİ

        Task AddSlideAsync(Slide slide);

        Task<List<Slide>> GetAllSlidesAsync();

        Task<Slide?> GetSlideByIdAsync(int id);

        Task UpdateSlideAsync(Slide slide);

        Task DeleteSlideAsync(Slide slide);

        List<Slide> GetActiveSlides();


        // SITE SOSYAL AYARLARI
        Task<SiteSocialAddressSetting?> GetSiteSocialSettingsAsync();
        Task UpdateSiteSocialSettingsAsync(SiteSocialAddressSetting setting);


        // KAMPANYA İŞLEMLERİ
        Task<List<Campaign>> GetAllCampaignsAsync();
        Task<Campaign?> GetCampaignByIdAsync(int id);

        Task<Campaign?> GetCampaignByUrlAsync(string url);

        Task AddCampaignAsync(Campaign campaign);
        Task UpdateCampaignAsync(Campaign campaign);
        Task DeleteCampaignAsync(Campaign campaign);

        // ADRES İŞLEMLERİ
        Task<List<Address>> GetAddressesByUserIdAsync(string userId);
        Task<Address?> GetAddressByIdAsync(int id);
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Address address);

        // SİPARİŞ İŞLEMLERİ
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task<List<Order>> GetAllOrdersAsync(); // admin için
        Task AddOrderAsync(Order order);

        Task CreateOrderAsync(Order order);

        

    }
}