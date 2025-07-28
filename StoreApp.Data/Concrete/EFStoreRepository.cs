using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;
using StoreApp.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Entities;




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
                .Include(p => p.Categories)
                .Where(p => p.Categories.Any(c => c.Url == category))
                .Count();
        }

        public IEnumerable<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            var products = Products;

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Include(p => p.Categories).Where(p => p.Categories.Any(c => c.Url == category));
            }

            return products.Skip((page - 1) * pageSize).Take(pageSize);


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

        //Campaign
        
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

    }

    
}