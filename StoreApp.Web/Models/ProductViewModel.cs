using StoreApp.Data.Entities;
namespace StoreApp.Web.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty;
    
    public bool IsFeatured { get; set; } = false;
    public bool IsBestSeller { get; set; } = false;
    public string Category { get; set; } = string.Empty;
}
public class ProductListViewModel
{
    public IEnumerable<ProductViewModel> Products { get; set; } = Enumerable.Empty<ProductViewModel>();

    public IEnumerable<ProductViewModel> FeaturedProducts { get; set; } = Enumerable.Empty<ProductViewModel>();
    public IEnumerable<ProductViewModel> BestSellerProducts { get; set; } = Enumerable.Empty<ProductViewModel>();

    public IEnumerable<Slide> Slides { get; set; } = Enumerable.Empty<Slide>();

    public List<Campaign> Campaigns { get; set; } = Enumerable.Empty<Campaign>().ToList();


    public PageInfo PageInfo { get; set; } = new();

}

public class PageInfo
{
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
}