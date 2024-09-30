public record CreateUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public string Phone { get; set; } = string.Empty;
    // address id 
}


