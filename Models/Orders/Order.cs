using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class Order{
    [Required]
    public Guid OrderId {get; set;}
    public DateTime OrderDate {get; set;}
    public decimal TotalAmount {get; set;}

 
     public Guid ShipmentId { get; set; }
    [JsonIgnore]
    public Shipment Shipment { get; set; }
   
}