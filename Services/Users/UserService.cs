
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    public Task<UserDto> GetUserByIdServiceAsync(Guid userId);
    public Task<User> CreateUserServiceAsync(CreateUserDto newUser);
    public Task<List<UserDto>> GetUsersServiceAsync();
    public Task<UserDto> UpdateUserByIdServiceAsync(Guid userId, UpdateUserDto updateUser);
    public Task<bool> DeleteUserByIdServiceAsync(Guid userId);


}
public class UserService : IUserService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;


    public UserService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }


    public async Task<User> CreateUserServiceAsync(CreateUserDto newUser)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser.Password = hashedPassword;

            var user = _mapper.Map<User>(newUser);

            await _appDbContext.Users.AddAsync(user);

            await _appDbContext.SaveChangesAsync();

            return user;

        }

        catch (DbUpdateException dbEx)
        {

            Console.WriteLine($"DbUpdateException: {dbEx.Message}\nStack Trace: {dbEx.StackTrace}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<List<UserDto>> GetUsersServiceAsync()
    {

        try
        {
            var users = await _appDbContext.Users.ToListAsync();
            var userIds = await _appDbContext.Users
            .Select(u => u.UserId)
            .ToListAsync();

            foreach (var item in userIds)
            {
                Console.WriteLine($"{item}");

            }

            var usersData = _mapper.Map<List<UserDto>>(users);
            return usersData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"DbUpdateException: {dbEx.Message}\nStack Trace: {dbEx.StackTrace}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<UserDto> GetUserByIdServiceAsync(Guid userId)
    {
        try
        {

            var user = await _appDbContext.Users.FindAsync(userId);
            var userData = _mapper.Map<UserDto>(user);
            return userData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"DbUpdateException: {dbEx.Message}\nStack Trace: {dbEx.StackTrace}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }


    public async Task<UserDto> UpdateUserByIdServiceAsync(Guid userId, UpdateUserDto updateUser)
    {
        try
        {
            var user = await _appDbContext.Users.FindAsync(userId);

            user.UserName = updateUser.UserName ?? user.UserName;
            user.Password = updateUser.Password ?? user.Password;
            user.Phone = updateUser.Phone ?? user.Phone;
            user.Image = updateUser.Image ?? user.Image;

            _appDbContext.Update(user);
            await _appDbContext.SaveChangesAsync();

            var userData = _mapper.Map<UserDto>(user);
            return userData;

        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"DbUpdateException: {dbEx.Message}\nStack Trace: {dbEx.StackTrace}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<bool> DeleteUserByIdServiceAsync(Guid userId)
    {
        try
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            _appDbContext.Remove(user);
            await _appDbContext.SaveChangesAsync();
            return true;

        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"DbUpdateException: {dbEx.Message}\nStack Trace: {dbEx.StackTrace}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }



}




