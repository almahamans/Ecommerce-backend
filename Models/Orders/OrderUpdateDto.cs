using System.ComponentModel.DataAnnotations;

public class OrderUpdateDto{
    [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be non-negative.")]
    public decimal? TotalAmount {get; set;}
    public List<OrderProduct>? OrderProducts { get; set; }
    public ShipmentStatus ShipmentStatus { get; set; }
}