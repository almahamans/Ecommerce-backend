

using System.Text.Json.Serialization;

public class Address
{

    public Guid AddresId { get; set; }
    public string City { get; set; }
    public string Neighberhood { get; set; }
    public string Street { get; set; }

   public Guid UserId  { get; set; }

   public User User { get; set; } 

}