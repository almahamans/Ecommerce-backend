public class CreateShipmentDto{
    public DateTime ShipmentDate { get; set; } = DateTime.UtcNow;
    public DateTime DeliveryDate { get; set; } = DateTime.UtcNow.AddDays(3);
    public required Guid TrackingNumber { get; set; }
}