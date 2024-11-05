public class QueryParameters
{
    public int pageNumber { get; set; } = 1;
    public int pageSize { get; set; } = 10;
    public string searchTerm { get; set; } = string.Empty;
    public string sortBy { get; set; } = string.Empty;
    public string sortOrder { get; set; } = "asc";
}