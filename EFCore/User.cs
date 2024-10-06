
public enum Role
{
    Admin,
    Customer
}

public record User
{

    public Guid UserId { get; set; }
    // public Guid AddressId { get; set; }
    //obj address
    public Role Role { get; set; } = Role.Customer;
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}