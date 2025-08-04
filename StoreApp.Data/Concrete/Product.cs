using StoreApp.Data.Helpers;
namespace StoreApp.Data.Concrete
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Url { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int Stock { get; set; }
        public bool IsFeatured { get; set; } = false;

        public bool IsBestSeller { get; set; } = false;
        public List<ProductCategory> ProductCategories { get; set; } = new();

        public Product()
        {
        }
        public Product(string name)
        {
            Name = name;
            Url = name.ToUrlSlug();
        }
        public bool IsApproved { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;


    }
}