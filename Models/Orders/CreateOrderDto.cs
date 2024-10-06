public class CreateOrderDto{
    public DateTime OrderDate { get; set; }
    public required decimal TotalAmount { get; set; }

    // public List<OrderProduct> orderProducts { get; set; }
}

