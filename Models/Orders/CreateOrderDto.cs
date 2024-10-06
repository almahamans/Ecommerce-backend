public class CreateOrderDto{
    public DateTime OrderDate { get; set; }
    public required decimal TotalAmount { get; set; }
    
        public Guid UserId { get; set; }


    // public List<OrderProduct> orderProducts { get; set; }
}

