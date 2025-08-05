using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;
using StoreApp.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Entities;
using StoreApp.Data.Location;






namespace StoreApp.Data.Concrete
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext _context;


        public EFStoreRepository(StoreDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> Products => _context.Products;
        public IQueryable<Category> Categories => _context.Categories;

        public IQueryable<Slide> Slides => _context.Slides;

        public IQueryable<Campaign> Campaigns => _context.Campaigns;

        public IQueryable<Address> Addresses => _context.Addresses;

        public IQueryable<Order> Orders => _context.Orders;
        public IQueryable<OrderItem> OrderItems => _context.OrderItems;

        public IQueryable<ProductCategory> ProductCategories => _context.ProductCategories;

        public IQueryable<Favorite> Favorites => _context.Favorites;


        // PRODUCT VE CATEGORY İŞLEMLERİ
        public void CreateProduct(Product entity)
        {
            throw new NotImplementedException();
        }

        public int GetProductCount(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return _context.Products.Count();
            }

            return _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Where(p => p.ProductCategories.Any(pc => pc.Category.Url == category))
                .Count();
        }


        public IEnumerable<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            var products = Products;

            if (!string.IsNullOrEmpty(category))
            {
                products = products
                    .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                    .Where(p => p.ProductCategories.Any(pc => pc.Category.Url == category));
            }

            return products
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task RemoveProductCategoriesAsync(IEnumerable<ProductCategory> items)
        {
            _context.ProductCategories.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Product> GetFeaturedProducts()
        {
            return _context.Products
                .Where(p => p.IsFeatured)
                .ToList();
        }

        public IEnumerable<Product> GetBestSellerProducts()
        {
            return _context.Products.
                Where(p => p.IsBestSeller)
                .ToList();
        }

        public async Task CreateCategoryAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCategoryAsync(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateProductAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product entity)
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        //SLİDE İŞLEMLERİ
        public List<Slide> GetActiveSlides()
        {
            return _context.Slides
                .Where(s => s.IsActive)
                .OrderBy(s => s.Order)
                .ToList();
        }

        public async Task<List<Slide>> GetAllSlidesAsync()
        {
            return await _context.Slides
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task<Slide?> GetSlideByIdAsync(int id)
        {
            return await _context.Slides.FindAsync(id);
        }

        public async Task AddSlideAsync(Slide slide)
        {
            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSlideAsync(Slide slide)
        {
            _context.Slides.Update(slide);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSlideAsync(Slide slide)
        {
            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
        }

        //Kampanya

        public async Task<List<Campaign>> GetAllCampaignsAsync()
        {
            return await _context.Campaigns.OrderByDescending(c => c.Id).ToListAsync();
        }

        public async Task<Campaign?> GetCampaignByIdAsync(int id)
        {
            return await _context.Campaigns.FindAsync(id);
        }

        public async Task AddCampaignAsync(Campaign campaign)
        {
            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCampaignAsync(Campaign campaign)
        {
            _context.Campaigns.Update(campaign);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCampaignAsync(Campaign campaign)
        {
            _context.Campaigns.Remove(campaign);
            await _context.SaveChangesAsync();
        }
        public async Task<Campaign?> GetCampaignByUrlAsync(string url)
        {
            return await _context.Campaigns
                .FirstOrDefaultAsync(c => c.Url.ToLower() == url.ToLower());
        }

        // Site Social Settings
        public async Task<SiteSocialAddressSetting?> GetSiteSocialSettingsAsync()
        {
            return await _context.SiteSocialAddressSettings.FirstOrDefaultAsync();
        }

        public async Task UpdateSiteSocialSettingsAsync(SiteSocialAddressSetting setting)
        {
            _context.SiteSocialAddressSettings.Update(setting);
            await _context.SaveChangesAsync();
        }

        //adress

        public async Task<List<Address>> GetAddressesByUserIdAsync(string userId)
        {
            return await _context.Addresses
                .Where(a => a.AppUserId == userId)
                .ToListAsync();
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task AddAddressAsync(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(Address address)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }


        //Sipariş İşlemleri

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.AppUserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.AppUser)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task CreateOrderAsync(Order order)
        {
            if (order == null || order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new ArgumentException("Geçersiz sipariş verisi.");
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        //
        public void RemoveProductCategory(ProductCategory pc)
        {
            _context.ProductCategories.Remove(pc);
        }
        public void AddProductCategory(ProductCategory pc)
        {
            _context.ProductCategories.Add(pc);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        //Favorite
        public async Task AddFavoriteAsync(string userId, int productId)
        {
            var exists = await _context.Favorites.AnyAsync(f => f.AppUserId == userId && f.ProductId == productId);
            if (!exists)
            {
                _context.Favorites.Add(new Favorite { AppUserId = userId, ProductId = productId });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFavoriteAsync(string userId, int productId)
        {
            var fav = await _context.Favorites
                .FirstOrDefaultAsync(f => f.AppUserId == userId && f.ProductId == productId);
            if (fav != null)
            {
                _context.Favorites.Remove(fav);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetUserFavoriteProductsAsync(string userId)
        {
            return await _context.Favorites
                .Where(f => f.AppUserId == userId)
                .Include(f => f.Product)
                .Select(f => f.Product)
                .ToListAsync();
        }
        public async Task<List<int>> GetFavoriteProductIdsAsync(string userId)
        {
            return await _context.Favorites
                .Where(f => f.AppUserId == userId)
                .Select(f => f.ProductId)
                .ToListAsync();
        }

        public async Task<bool> IsProductFavoritedAsync(string userId, int productId)
        {
            return await _context.Favorites.AnyAsync(f => f.AppUserId == userId && f.ProductId == productId);
        }








    }


}