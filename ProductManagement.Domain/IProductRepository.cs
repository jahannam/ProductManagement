namespace ProductManagement.Domain
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProducts();
        Task<Product> SaveProduct(Product product);
        Task<bool> AreProductCodeAndNameBothUnique(string productCode, string productName);
    }
}
