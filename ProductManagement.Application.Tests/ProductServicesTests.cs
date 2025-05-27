using Microsoft.Extensions.Logging;
using NSubstitute;
using ProductManagement.Domain;
using Shouldly;

namespace ProductManagement.Application.Tests
{
    public class ProductServicesTests
    {
        private readonly IProductRepository _productRepositoryMock;
        private readonly IProductService _productService;

        public ProductServicesTests()
        {
            _productRepositoryMock = Substitute.For<IProductRepository>();

            _productService = new ProductService(
                _productRepositoryMock,
                Substitute.For<ILogger<ProductService>>());
        }

        [Fact]
        public async Task GetProducts()
        {
            var products = new List<Product>()
            {
                new () { Id = 1, ProductCode = "P001", Name = "Product 1",
                    Category = "Tool", Price = 7.6M, Quantity = 1, DateAdded = DateTimeOffset.UtcNow },
                new () { Id = 2, ProductCode = "P002", Name = "Product 2",
                    Category = "Tool2", Price = 3.5M, Quantity = 1, DateAdded = DateTimeOffset.UtcNow.AddDays(-1) },
            };
            _productRepositoryMock.GetProducts().Returns(products);
            var result = await _productService.GetProducts();
            result.ShouldBe(products);
        }

        [Fact]
        public async Task SaveProducts_HappyPath()
        {
            var product = new Product
            {
                ProductCode = "P001",
                Name = "Product 1",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1
            };
            var returnProduct = new Product
            {
                Id = 1,
                ProductCode = "P001",
                Name = "Product 1",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1,
                DateAdded = DateTimeOffset.UtcNow
            };
            _productRepositoryMock.AreProductCodeAndNameBothUnique(product.ProductCode, product.Name).Returns(true);
            _productRepositoryMock.SaveProduct(product).Returns(returnProduct);
            var result = await _productService.SaveProduct(product);
            result.Success.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Message.ShouldBeNull();
            result.Data.ShouldBe(returnProduct);
        }

        [Fact]
        public async Task SaveProducts_CodeOrNameNotUnique()
        {
            var product = new Product
            {
                ProductCode = "P001",
                Name = "Product 1",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1
            };
            _productRepositoryMock.AreProductCodeAndNameBothUnique(product.ProductCode, product.Name).Returns(false);
            var result = await _productService.SaveProduct(product);
            result.Success.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Message.ShouldBe("Product code and name must be unique.");
        }
    }
}
