public enum ShipmentStatus{
    Pending,
    Shipped,
    Delivered,
    Canceled,
    Returned,
    OnProgress
}
public class Shipment{
public required Guid ShipmentId {get; set;}
public ShipmentStatus ShipmentStatus { get; set; }
public DateTime ShipmentDate {get; set;} 
public DateTime DeliveryDate {get; set;} = DateTime.UtcNow.AddDays(3);
public required Guid TrackingNumber {get; set;} 
}