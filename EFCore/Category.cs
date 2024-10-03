using System.Text.Json.Serialization;

public record Category
{

    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    // slug is for creating a readable URL.
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 1-m --> one category for many products
    [JsonIgnore]
    public List<Product> Products { get; set; }
}