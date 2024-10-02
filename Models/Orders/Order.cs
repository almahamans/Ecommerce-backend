public enum OrderStatus{
    Pending,
    Shipped,
    Delivered,
    Canceled,
    Returned,
    OnProgress
}
public class Order{
    public Guid OrderId {get; set;}
    public OrderStatus OrderStatus {get; set;}
    public DateTime OrderDate {get; set;} 
    public decimal TotalAmount {get; set;}
    public int Quantity { get; set; }
    public string Image {get; set;} = string.Empty;

}