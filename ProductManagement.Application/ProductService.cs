using Microsoft.Extensions.Logging;
using ProductManagement.Domain;

namespace ProductManagement.Application
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Product>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return products;
        }

        public async Task<Result<Product>> SaveProduct(Product productToSave)
        {
            bool isUnique = await _productRepository.AreProductCodeAndNameBothUnique(productToSave.ProductCode, productToSave.Name);
            if (!isUnique)
            {
                return Result<Product>.Fail("Product code and name must be unique.");
            }                

            var newProduct = await _productRepository.SaveProduct(productToSave);

            return Result<Product>.Ok(newProduct);
        }
    }
}
