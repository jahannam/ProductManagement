using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain;

namespace ProductManagement.Infrastructure
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }
    }
}
