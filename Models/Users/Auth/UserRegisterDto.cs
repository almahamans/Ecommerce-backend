    
    using System.ComponentModel.DataAnnotations;
    public class UserRegisterDto {
    
    [Required(ErrorMessage = "Username is missing.")]
    [StringLength(50, ErrorMessage = "Username must be between 3 and 50 characters.", MinimumLength = 3)]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters.", MinimumLength = 6)]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public required string Email { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Image { get; set; }}
