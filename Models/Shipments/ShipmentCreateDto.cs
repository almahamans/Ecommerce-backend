public class ShipmentCreateDto{
    public DateTime ShipmentDate { get; set; }
    public DateTime DeliveryDate { get; set; } = DateTime.UtcNow.AddDays(3);
}