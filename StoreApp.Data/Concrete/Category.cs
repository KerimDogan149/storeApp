namespace StoreApp.Data.Concrete
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public string? Image { get; set; }

        public List<ProductCategory> ProductCategories { get; set; } = new();



    }
}