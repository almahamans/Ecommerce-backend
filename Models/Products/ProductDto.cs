public record ProductDto
{
    public required string ProductName { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required string Image { get; set; }

}