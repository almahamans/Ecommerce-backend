public class OrderProductCreateDto{
    public int ProductQuantity { get; set; }
    public decimal ProductsPrice { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
}