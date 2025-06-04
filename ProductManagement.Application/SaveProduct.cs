namespace ProductManagement.Application
{
    public record SaveProduct
    {
        public required string Category { get; init; }
        public required string Name { get; init; }
        public required string ProductCode { get; init; }
        public required decimal Price { get; init; }
        public required int Quantity { get; init; }
    }
}
