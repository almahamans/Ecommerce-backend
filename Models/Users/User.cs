
public enum Role
{
    Admin,
    Customer
}

public record User
{
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
    public Role Role { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}