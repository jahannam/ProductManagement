using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;
        
        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Product>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> SaveProduct(Product product)
        {
            _dbContext.Products.Add(product);
            product = product with { DateAdded = DateTimeOffset.UtcNow };  
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> AreProductCodeAndNameBothUnique(string productCode, string productName)
        {
            return !await _dbContext.Products
                .AnyAsync(p => p.ProductCode == productCode || p.Name == productName);
        }
    }
}
