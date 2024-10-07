using System.Text.Json.Serialization;

public class OrderProduct{
    public int ProductQuantity {get; set;}
    public decimal ProductsPrice { get; set; }
    public Guid OrderId { get; set; }
    [JsonIgnore]
    public Order? Order { get; set; }
    public Guid ProductId { get; set; }
    [JsonIgnore]
    public Product? Product { get; set; }
}