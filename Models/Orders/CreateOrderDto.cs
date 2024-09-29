public class CreateOrderDto{
    public OrderStatus orderStatus { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public required decimal TotalAmount { get; set; }
    public string Image { get; set; } = string.Empty;
}