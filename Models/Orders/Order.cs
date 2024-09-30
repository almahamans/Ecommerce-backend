public enum OrderStatus{
    Delevierd,
    Returned,
    Shipped,
    OnProgress
}
public class Order{
    public Guid OrderId {get; set;}
    public OrderStatus OrderStatus {get; set;} = OrderStatus.OnProgress;
    public DateTime OrderDate {get; set;} = DateTime.UtcNow;
    public decimal TotalAmount {get; set;}
    public string Image {get; set;} = string.Empty;
}