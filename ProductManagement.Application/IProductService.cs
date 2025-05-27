using ProductManagement.Domain;

namespace ProductManagement.Application
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProducts();
        Task<Result<Product>> SaveProduct(Product product);
    }
}