public record ProductDto
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required string Image { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public CategoryDto? category { get; set; }
    public List<OrderProduct> OrderProducts { get; set; }
}