 public class UserService
  {
      private readonly AppDbContext _appDbContext;

      public UserService(AppDbContext appDbContext){
          _appDbContext = appDbContext;
      }


      public async Task<User> CreateUserAsync(CreateUserDto newUserDto)
{
    var user = new User
    {
        UserId = Guid.NewGuid(),
        UserName = newUserDto.UserName,
        Email = newUserDto.Email,
        Password = newUserDto.Password, 
        Role = newUserDto.Role,
    };

    await _appDbContext.Users.AddAsync(user);
    await _appDbContext.SaveChangesAsync();
    return user;
}

  }