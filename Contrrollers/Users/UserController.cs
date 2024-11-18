
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController, Route("/api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;

    }

    [Authorize(Roles = "Admin")] 
    [HttpPut("create-admin/{userId}")]
    public async Task<IActionResult> UpdateToAdmin(Guid userId)
    {
        var user = await _userService.UpdateToAdminByIdServiceAsync(userId);
        if (user == null)
        {
            return ApiResponse.NotFound($"User with this id {userId} does not exist");
        }
        return ApiResponse.Success(user, "User is now is Admin");
    }
   [Authorize(Roles = "Admin, Customer")]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] QueryParameters queryParameters)
    {
        try
        {
            var users = await _userService.GetUsersSearchByServiceAsync(queryParameters);
            return ApiResponse.Success(users);
        }
        catch (ApplicationException ex)
        {
            return ApiResponse.ServerError(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception : {ex.Message}");
            return ApiResponse.ServerError("An unexpected error occurred.");
        }
    }
    [Authorize(Roles = "Admin, Customer")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _userService.GetUserByIdServiceAsync(userId);
        if (user == null)
        {
            return ApiResponse.NotFound($"User with this id {userId} does not exist");
        }
        return ApiResponse.Success(user, "User is returned succesfully");
    }
    [Authorize(Roles = "Customer")]
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            return ApiResponse.BadRequest("Invalid User Data");
        }
        var user = await _userService.UpdateUserByIdServiceAsync(userId, updateUserDto);
        if (user == null)
        {
            return ApiResponse.BadRequest($" User with this {userId} does not exist");

        }
        return ApiResponse.Success(user, "User is Updated succesfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var isDeleted = await _userService.DeleteUserByIdServiceAsync(userId);
        if (isDeleted == false)
        {
            return ApiResponse.NotFound($"User with this id {userId} does not exist");
        }
        return ApiResponse.Success(isDeleted, "User is Deleted succesfully");
    }
}