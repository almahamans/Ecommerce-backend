using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public enum ShipmentStatus{
    Pending = 5,
    Shipped = 1,
    Delivered = 2,
    Canceled = 3,
    Returned = 4,
    OnProgress = 0
}
public class Shipment{
    [Required]
    public Guid ShipmentId { get; set; }
    public ShipmentStatus ShipmentStatus { get; set; }
    public DateTime ShipmentDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Guid OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }
}