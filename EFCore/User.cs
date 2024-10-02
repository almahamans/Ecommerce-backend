
using System.Text.Json.Serialization;

public enum Role
{
    Admin,
    Customer
}
public class User
{
    public Guid UserId { get; set; }
    public Role Role { get; set; } = Role.Customer;
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   //  public Guid AddressId { get; set; }

   // [JsonIgnore]
   //    public List<Address> Addresses { get; set; } = new List<Address>();

}