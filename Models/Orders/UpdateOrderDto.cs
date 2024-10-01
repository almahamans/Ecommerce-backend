public class UpdateOrderDto{
    public OrderStatus? orderStatus {get; set;}
    public decimal? TotalAmount {get; set;}
    public int? Quantity { get; set; }
}