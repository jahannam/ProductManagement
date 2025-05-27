namespace ProductManagement.Domain
{
    public record Product
    {
        public int Id { get; set; }
        public required string Category { get; init; }
        public required string Name { get; init; }
        public required string ProductCode { get; init; }
        public required decimal Price { get; init; }
        public required int Quantity { get; init; }
        public required DateTimeOffset DateAdded { get; init; }
    }
}
