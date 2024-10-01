
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
//
public interface IAddressService
{
public Task<PaginatedResult<Address>> GetAddressesSearchByServiceAsync(QueryParameters queryParameters);

    public Task<PaginatedResult<Address>> GetAddressesPaginationServiceAsync(int pageNumber, int pageSize);
    public Task<List<Address>> GetAddressesServiceAsync();
    public Task<AddressDto> GetAddressByIdServiceAsync(Guid addressId);
 public Task<AddressDto> CreateAddressServiceAsync(CreateAddressDto newAddress);
 public Task<AddressDto> UpdateAddressByIdServiceAsync(Guid addressId, UpdateAddressDto updateAddress);
    public Task<bool> DeleteAddressByIdServiceAsync(Guid addressId);
}
public class AddressService : IAddressService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;


    public AddressService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<Address>> GetAddressesSearchByServiceAsync(QueryParameters queryParameters)
    {
        try
        {
            var query = _appDbContext.Addresses.AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
            {
                var lowerCaseSearchTerm = queryParameters.SearchTerm.ToLower();
                query = query.Where(a => a.City.ToLower().Contains(lowerCaseSearchTerm));
            }

            if (queryParameters.SortBy?.ToLower()== "city"){
            query = queryParameters.SortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.City)
                        : query.OrderBy(u => u.City);
            }

            var totalCount = await query.CountAsync();

            if (queryParameters.PageNumber < 1) queryParameters.PageNumber = 1;
            if (queryParameters.PageSize < 1) queryParameters.PageSize = 10;

            var addresses = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync(); 

            return new PaginatedResult<Address>
            {
                Items = addresses,
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

    public async Task<PaginatedResult<Address>> GetAddressesPaginationServiceAsync(int pageNumber, int pageSize)
    {

        try
        {
            var totalCount = await _appDbContext.Users.CountAsync();

            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var addresses = await _appDbContext.Addresses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Address>
            {
                Items = addresses,
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

    public async Task<List<Address>> GetAddressesServiceAsync()
    {

        try
        {
            var addresses = await _appDbContext.Addresses.ToListAsync();
            return addresses;
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

    public async Task<AddressDto> GetAddressByIdServiceAsync(Guid addressId)
    {
        try
        {

            var address = await _appDbContext.Addresses.FindAsync(addressId);
            var addressData = _mapper.Map<AddressDto>(address);
            return addressData;
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
    public async Task<AddressDto> CreateAddressServiceAsync(CreateAddressDto newAddress)
    {
        try
        {
            Console.WriteLine($"----------ser---------------");
            
           
            var address = _mapper.Map<Address>(newAddress);

            await _appDbContext.Addresses.AddAsync(address);

            await _appDbContext.SaveChangesAsync();


            var addressData = _mapper.Map<AddressDto>(address);
            return addressData;

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

    public async Task<AddressDto> UpdateAddressByIdServiceAsync(Guid addressId, UpdateAddressDto updateAddress)
    {
        try
        {
            var address = await _appDbContext.Addresses.FindAsync(addressId);

            address.City = updateAddress.City ??  address.City;
           address.Neighberhood = updateAddress.Neighberhood ??  address.Neighberhood;
            address.Street = updateAddress.Street ??  address.Street;

            _appDbContext.Update(address);
            await _appDbContext.SaveChangesAsync();

            var addressData = _mapper.Map<AddressDto>(address);
            return addressData;

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

    public async Task<bool> DeleteAddressByIdServiceAsync(Guid addressId)
    {
        try
        {
            var address = await _appDbContext.Addresses.FindAsync(addressId);
            if (address == null)
            {
                return false;
            }

            _appDbContext.Remove(address);
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




