public class OrderDto{
    public Guid OrderId { get; set; }
    public OrderStatus orderStatus { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public string Image { get; set; } = string.Empty;
}