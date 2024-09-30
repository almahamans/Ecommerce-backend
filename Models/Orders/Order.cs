public enum OrderStatus{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Canceled,
    Returned,
    OnProgress
}
public class Order{
    public Guid OrderId {get; set;}
    public OrderStatus OrderStatus {get; set;}
    public DateTime OrderDate {get; set;} = DateTime.Now;
    public decimal TotalAmount {get; set;}
    public string Image {get; set;} = string.Empty;
}