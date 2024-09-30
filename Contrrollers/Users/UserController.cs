
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController, Route("/api/v1/users")]
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
        try
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse.BadRequest("Invalid User Data");
            }
            var user = await _userService.CreateUserServiceAsync(newUser);
            return ApiResponse.Created(user, "user created successfully");
        }

        catch (ApplicationException ex)
        {
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = _userService.GetUsersServiceAsync();
        return ApiResponse.Success(users, "Users are returned succesfully");
    }
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        Console.WriteLine($"In controller----------");

        var user = await _userService.GetUserByIdServiceAsync(userId);
        return ApiResponse.Success(user, "User is returned succesfully");
    }
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = await _userService.UpdateUserByIdServiceAsync(userId, updateUserDto);
        return ApiResponse.Success(user, "User is Updated succesfully");

    }
    [HttpDelete("{userId}")]

    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var user = await _userService.DeleteUserByIdServiceAsync(userId);
        return ApiResponse.Success(user, "User is Deleted succesfully");
    }

}