
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    public Task<PaginatedResult<User>> GetUsersSearchByServiceAsync(QueryParameters queryParameters);

    public Task<PaginatedResult<User>> GetUsersPaginationServiceAsync(int pageNumber, int pageSize);
    public Task<List<User>> GetUsersServiceAsync();
    public Task<UserDto> GetUserByIdServiceAsync(Guid userId);
    public Task<UserDto> CreateUserServiceAsync(CreateUserDto newUser);
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

    public async Task<PaginatedResult<User>> GetUsersSearchByServiceAsync(QueryParameters queryParameters)
    {
        try
        {
            var query = _appDbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
            {
                var lowerCaseSearchTerm = queryParameters.SearchTerm.ToLower();
                query = query.Where(u => u.UserName.ToLower().Contains(lowerCaseSearchTerm));
            }

            switch (queryParameters.SortBy?.ToLower())
            {
                case "UserName":
                    query = queryParameters.SortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.UserName)
                        : query.OrderBy(u => u.UserName);
                    break;
                case "CreatedAt":
                    query = queryParameters.SortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.CreatedAt)
                        : query.OrderBy(u => u.CreatedAt);
                    break;

                default:
                    query = query.OrderBy(u => u.UserName); 
                    break;
            }

            var totalCount = await query.CountAsync();

            if (queryParameters.PageNumber < 1) queryParameters.PageNumber = 1;
            if (queryParameters.PageSize < 1) queryParameters.PageSize = 10;

            var users = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize) 
                .Take(queryParameters.PageSize)                   
                .ToListAsync();

           

            return new PaginatedResult<User>
            {
                Items = users,                  
                TotalCount = totalCount,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later." + ex.Message);
        }
    }

    public async Task<PaginatedResult<User>> GetUsersPaginationServiceAsync(int pageNumber, int pageSize)
    {

        try
        {
            var totalCount = await _appDbContext.Users.CountAsync();

            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var users = await _appDbContext.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<User>
            {
                Items = users,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later." + ex.Message);
        }
    }

    public async Task<List<User>> GetUsersServiceAsync()
    {

        try
        {
            var users = await _appDbContext.Users.ToListAsync();
            return users;
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
    public async Task<UserDto> CreateUserServiceAsync(CreateUserDto newUser)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser.Password = hashedPassword;

            var user = _mapper.Map<User>(newUser);

            await _appDbContext.Users.AddAsync(user);

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




