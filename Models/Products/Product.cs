using System.Runtime.Intrinsics.X86;

public record Product
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
    public required string Image { get; set; }

}