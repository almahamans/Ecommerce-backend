public class OrderDto
{
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid ShipmentId { get; set; }
    public ShipmentStatus shipmentStatus { get; set; }
    public User User { get; set; }

    public List<OrderProduct> OrderProducts { get; set; }
}