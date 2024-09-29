public enum OrderStatus{
    Delevierd,
    Returned,
    Shipped,
    OnProgress
}
public class Order{
    public Guid OrderId {get; set;}
    public OrderStatus OrderStatus {get; set;}
    public DateTime OrderDate {get; set;} = DateTime.Now;
    public decimal TotalAmount {get; set;}
    public string Image {get; set;} = string.Empty;
}