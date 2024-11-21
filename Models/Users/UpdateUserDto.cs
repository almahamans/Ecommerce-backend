
using System.ComponentModel.DataAnnotations;


public class UpdateUserDto
{

    [StringLength(50, ErrorMessage = "Username must be between 3 and 50 characters.", MinimumLength = 3)]
    public string? UserName { get; set; }

    [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters.", MinimumLength = 6)]
    public string? Password { get; set; }
    public string? Phone { get; set; }
      public string? Email { get; set; }
}