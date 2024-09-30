public record UpdateProductDto
{
    public required string ProductName { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
    public required string Image { get; set; }

}