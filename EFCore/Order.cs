using System.ComponentModel.DataAnnotations;
public class Order{
    [Required]
    public Guid OrderId {get; set;}
    public DateTime OrderDate {get; set;}
    public decimal TotalAmount {get; set;}
}