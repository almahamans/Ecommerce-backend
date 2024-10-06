public class CreateOrderDto{
    public DateTime OrderDate { get; set; }
    public required decimal TotalAmount { get; set; }
    public Guid UserId { get; set; }
    public List<CreateOrderProductDto> OrderProducts { get; set; } = new List<CreateOrderProductDto>();
}

