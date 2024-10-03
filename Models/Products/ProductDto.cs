public record ProductDto
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required string Image { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }  //fk

}