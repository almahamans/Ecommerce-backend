public record UserDto
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public string Phone { get; set; } = string.Empty;
   
}