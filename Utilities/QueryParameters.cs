public class QueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SearchTerm { get; set; } = string.Empty;
    public string SortBy { get; set; } = "Name"; 
    public string SortOrder { get; set; } = "asc";
}