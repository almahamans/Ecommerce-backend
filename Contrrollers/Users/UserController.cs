
using Microsoft.AspNetCore.Mvc;


[ApiController, Route("/api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto newUser)
    {
        var user = await _userService.CreateUserAsync(newUser);
        var response = new { Message = "An user created successfully", User = user };
        return Created($"/api/users/{user.UserId}", response);
    }
    
}