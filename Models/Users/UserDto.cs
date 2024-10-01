public class UserDto
{
    public Role Role {get; set;}
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
      public string? Image { get; set; }
    //addrss id - user id
   
}