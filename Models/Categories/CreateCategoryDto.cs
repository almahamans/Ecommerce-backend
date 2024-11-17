public record CreateCategoryDto{
public required string CategoryName { get; set; }
public string Slug { get; set; } = string.Empty;
}