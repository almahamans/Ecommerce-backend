public record CategoryWithProductsDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public DateTime CreatedAt { get; set; }

    // List of products in the category
     public List<ProductDto>? Products { get; set; }

}