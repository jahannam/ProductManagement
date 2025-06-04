using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ProductManagement.Api.Controllers;
using ProductManagement.Application;
using ProductManagement.Domain;
using Shouldly;

namespace ProductManagement.Api.Tests
{
    public class ProductControllerTests
    {
        private readonly IProductService _productServiceMock;
        private readonly IValidator<SaveProduct> _productValidatorMock;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            _productServiceMock = Substitute.For<IProductService>();
            _productValidatorMock = Substitute.For<IValidator<SaveProduct>>();

            _productController = new ProductController(
                Substitute.For<ILogger<ProductController>>(),
                _productServiceMock,
                _productValidatorMock);
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
            _productServiceMock.GetProducts().Returns(products);
            var result = await _productController.GetProducts();
            result.ShouldBe(products);
        }

        [Fact]
        public async Task SaveProduct_HappyPath()
        {
            var product = new SaveProduct
            {
                ProductCode = "P001",
                Name = "Product 1",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1
            };
            var productResult = Result<Product>.Ok(new Product
            {
                Id = 1,
                ProductCode = "P001",
                Name = "Product 1",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1,
                DateAdded = DateTimeOffset.UtcNow
            });
            _productValidatorMock.ValidateAsync(product).Returns(new ValidationResult());
            _productServiceMock.SaveProduct(product).Returns(productResult);
            var result = await _productController.SaveProduct(product);
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.ShouldNotBeNull();
            okResult.Value.ShouldNotBeNull();
            okResult.Value.ShouldBe(productResult.Data);
        }

        [Fact]
        public async Task SaveProduct_ValidationFailure()
        {
            var product = new SaveProduct
            {
                ProductCode = "",
                Name = "",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1
            };

            var errors = new List<ValidationFailure>
            {
                new ValidationFailure("ProductCode", "Product code is required"),
                new ValidationFailure("Name", "Name is required")
            };

            _productValidatorMock.ValidateAsync(product).Returns(new ValidationResult(errors));
            var result = await _productController.SaveProduct(product);
            result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.ShouldNotBeNull();
            badRequestResult.Value.ShouldNotBeNull();
            badRequestResult.Value.ShouldBe(errors);
        }

        [Fact]
        public async Task SaveProduct_SavingFailure()
        {
            var product = new SaveProduct
            {
                ProductCode = "P001",
                Name = "Product 1",
                Category = "Tool",
                Price = 7.6M,
                Quantity = 1
            };
            var productResult = Result<Product>.Fail("Product code and name must be unique.");
            _productValidatorMock.ValidateAsync(product).Returns(new ValidationResult());
            _productServiceMock.SaveProduct(product).Returns(productResult);
            var result = await _productController.SaveProduct(product);
            result.ShouldBeOfType<ConflictObjectResult>();
            var confilictResult = result as ConflictObjectResult;
            confilictResult.ShouldNotBeNull();
            confilictResult.Value.ShouldNotBeNull();
            confilictResult.Value.ShouldBe(productResult.Message);
        }
    }
}
