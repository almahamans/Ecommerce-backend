public record CreateProductDto
{
    public required string ProductName { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public string Slug { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Image { get; set; }
    public Guid CategoryId { get; set; } // fk for category
    public List<OrderProductCreateDto> OrderProducts { get; set; } = new List<OrderProductCreateDto>();
}