using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StoreApp.Data.Concrete;
using StoreApp.Data.Entities;

namespace StoreApp.Data.Concrete
{
    public class StoreDbContextFactory : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
            optionsBuilder.UseSqlite("Data Source=store.db");

            return new StoreDbContext(optionsBuilder.Options);
        }
    }
}
