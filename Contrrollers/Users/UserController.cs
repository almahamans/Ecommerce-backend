
using System.Text;
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
    // GET => /api/users => Get all the users
    [HttpGet("Searching")]
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

    [HttpGet("paginated")]
    public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
    {
        try
        {
            var users = await _userService.GetUsersPaginationServiceAsync(pageNumber, pageSize);
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
        try
        {
            var users = await _userService.GetUsersServiceAsync();
            if (users == null || !users.Any())
            {
                return ApiResponse.NotFound("There are no users yet");
            }
            return ApiResponse.Success(users, "Users are returned succesfully");
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
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            return ApiResponse.BadRequest("Invalid User Data");
        }
        var user = await _userService.UpdateUserByIdServiceAsync(userId, updateUserDto);
        return ApiResponse.Success(user, "User is Updated succesfully");

    }
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