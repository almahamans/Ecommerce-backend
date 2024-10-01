
using System.ComponentModel.DataAnnotations;

public record AddressDto
{
    [StringLength(50, ErrorMessage = "City must be between 3 and 50 characters.", MinimumLength = 3)]
    public  string? City { get; set; }

    [StringLength(100, ErrorMessage = "Neighberhood must be between 3 and 100 characters.", MinimumLength = 6)]
    public  string? Neighberhood { get; set; }

    [StringLength(100, ErrorMessage = "street must be between 3 and 100 characters.", MinimumLength = 6)]
    public  string? Street { get; set; }

}


