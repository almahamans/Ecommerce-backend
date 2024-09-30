public class CreateOrderDto{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public required decimal TotalAmount { get; set; }
    public string Image { get; set; } = string.Empty;
}