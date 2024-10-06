public class UpdateOrderDto{
    public decimal? TotalAmount {get; set;}
    public List<OrderProduct>? orderProducts { get; set; }
}