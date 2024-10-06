using System.Runtime.Intrinsics.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
public record Product
{
    [Key]
    public Guid ProductId { get; set; }

    [Required]
    [StringLength(150)]
    public string ProductName { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    // slug is for creating a readable URL.
    public string Slug { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; } = 0;

     [Range(0, int.MaxValue)]
    public int Quantity { get; set; } = 0;
    public string Image { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // FK
    public Guid CategoryId { get; set; }

    //category refrence
    //converting the refernce to JSON can create a loop. 
    //Using [JsonIgnore] on one side of the relationship helps break this loop.
    [JsonIgnore]
    public Category? Category { get; set; }


}