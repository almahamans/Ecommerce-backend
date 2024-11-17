using System.ComponentModel.DataAnnotations;

public class OrderCreateDto{
    public DateTime OrderDate { get; set; }
    public required decimal TotalAmount { get; set; }
    public Guid UserId { get; set; }
    public List<OrderProductCreateDto> OrderProducts { get; set; } = new List<OrderProductCreateDto>();
}

