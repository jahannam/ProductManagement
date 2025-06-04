using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain;
using Shouldly;

namespace ProductManagement.Infrastructure.Tests
{
    public class ProductRepositoryTests : IDisposable 
    {
        private readonly ProductDbContext _context;
        private readonly SqliteConnection _connection;
        private readonly IProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            // Create in-memory SQLite connection
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new ProductDbContext(options);
            _context.Database.EnsureCreated();
            _productRepository = new ProductRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _connection.Close();
        }

        [Fact]
        public async Task GetProducts_HappyPath()
        {
            var products = new List<Product>()
            {
                new () { Id = 1, ProductCode = "P001", Name = "Product 1",
                    Category = "Tool", Price = 7.6M, Quantity = 1, DateAdded = DateTimeOffset.UtcNow },
                new () { Id = 2, ProductCode = "P002", Name = "Product 2",
                    Category = "Tool2", Price = 3.5M, Quantity = 1, DateAdded = DateTimeOffset.UtcNow.AddDays(-1) },
            };
            _context.Add(products[0]);
            _context.Add(products[1]);
            await _context.SaveChangesAsync();
            var result = await _productRepository.GetProducts();
            result.ShouldBe(products);
        }

        [Fact]
        public async Task SaveProduct_HappyPath()
        {
            var product = new Product
            {
                ProductCode = "P001",
                Name = "Product 1",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1,
                DateAdded = DateTimeOffset.UtcNow
            };
            var result = await _productRepository.SaveProduct(product);
            result.ShouldBe(product);
        }

        [Fact]
        public async Task AreProductCodeAndNameBothUnique_HappyPath()
        {
            var result = await _productRepository.AreProductCodeAndNameBothUnique("Unique", "Unique");
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AreProductCodeAndNameBothUnique_NotUnique()
        {
            var product = new Product
            {
                Id = 15,
                ProductCode = "P001",
                Name = "Product 1",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1,
                DateAdded = DateTimeOffset.UtcNow
            };
            _context.Add(product);
            await _context.SaveChangesAsync();
            var result = await _productRepository.AreProductCodeAndNameBothUnique(product.ProductCode, product.Name);
            result.ShouldBeFalse();
        }
    }
}
