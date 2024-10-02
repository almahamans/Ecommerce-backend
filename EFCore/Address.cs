

using System.Text.Json.Serialization;

public class Address
{

    public Guid AddresId { get; set; }
    public string City { get; set; }
    public string Neighberhood { get; set; }
    public string Street { get; set; }

    //req
  //  public Guid UserId  { get; set; }

  //   [JsonIgnore]
 //   public User User { get; set; } = new User();

}