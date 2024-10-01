public class ShipmentDto{
    public DateTime ShipmentDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Guid TrackingNumber { get; set; }
    public ShipmentStatus ShipmentStatus { get; set; }
}