public record CategoryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public DateTime CreatedAt { get; set; }

}