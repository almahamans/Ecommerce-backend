public class CreateOrderDto{
    public OrderStatus orderStatus { get; set; } = OrderStatus.OnProgress;
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public required decimal TotalAmount { get; set; }
    public string Image { get; set; } = string.Empty;
}