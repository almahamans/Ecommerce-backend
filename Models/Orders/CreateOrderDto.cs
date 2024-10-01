public class CreateOrderDto{
    public DateTime OrderDate { get; set; }
    public required decimal TotalAmount { get; set; }
    public string Image { get; set; } = string.Empty;
    public int Quantity { get; set; }
}