public class OrderDto{
    public DateTime OrderDate { get; set; } 
    public decimal TotalAmount { get; set; }
    public Guid ShipmentId { get; set; }
    public ShipmentStatus shipmentStatus {get; set;}
}