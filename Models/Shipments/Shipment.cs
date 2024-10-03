using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public enum ShipmentStatus
{
    Pending,
    Shipped,
    Delivered,
    Canceled,
    Returned,
    OnProgress
}
public class Shipment{
    [Required]
    public Guid ShipmentId { get; set; }
    public ShipmentStatus ShipmentStatus { get; set; }
    public DateTime ShipmentDate { get; set; }
    public DateTime DeliveryDate { get; set; }
 //not really important
    public Guid OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }
    
    
}