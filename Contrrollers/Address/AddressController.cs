
// using System.Text;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;


// [ApiController, Route("/api/v1/address")]
// public class AddressController : ControllerBase
// {
//     private readonly IAddressService _addressService;
//     public AddressController(IAddressService addressService)
//     {
//         _addressService = addressService;
//     }

//     [Authorize(Roles = "Admin")]
//     [HttpGet("Searching")]
//     public async Task<IActionResult> GetAddresses([FromQuery] QueryParameters queryParameters)
//     {
//         try
//         {
//             var address = await _addressService.GetAddressesSearchByServiceAsync(queryParameters);
//             //if theer is no output
//             // if (address == null)
//             // {
//             //     return ApiResponse.NotFound("There are no address yet");
//             // }
//             return ApiResponse.Success(address);
//         }
//         catch (ApplicationException ex)
//         {
//             return ApiResponse.ServerError(ex.Message);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Exception : {ex.Message}");
//             return ApiResponse.ServerError("An unexpected error occurred.");
//         }
//     }

//     [Authorize(Roles = "Admin")]
//     [HttpGet("paginated")]
//     public async Task<IActionResult> GetAddress([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
//     {
//         try
//         {
//             var addresses = await _addressService.GetAddressesPaginationServiceAsync(pageNumber, pageSize);
//             return ApiResponse.Success(addresses);
//         }
//         catch (ApplicationException ex)
//         {
//             return ApiResponse.ServerError(ex.Message);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Exception : {ex.Message}");
//             return ApiResponse.ServerError("An unexpected error occurred.");
//         }
//     }
//     //customer have to be loged in to create an address
//     [Authorize(Roles = "Customer")]
//     [HttpPost]
//     public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto newAddress)
//     {
//         Console.WriteLine($"-----cont----------------");


//         try
//         {
//             if (!ModelState.IsValid)
//             {
//                 return ApiResponse.BadRequest("Invalid Address Data");
//             }
//             var address = await _addressService.CreateAddressServiceAsync(newAddress);
//             return ApiResponse.Created(address, "Address created successfully");
//         }

//         catch (ApplicationException ex)
//         {

//             return ApiResponse.ServerError("Server error: " + ex.Message);
//         }
//         catch (System.Exception ex)
//         {
//             return ApiResponse.ServerError("Server error: " + ex.Message);
//         }
//     }
//     [Authorize(Roles = "Admin")]
//     [HttpGet]
//     public async Task<IActionResult> GetAddresses()
//     {
//         try
//         {
//             var address = await _addressService.GetAddressesServiceAsync();
//             if (address == null || !address.Any())
//             {
//                 return ApiResponse.NotFound("There are no address yet");
//             }
//             return ApiResponse.Success(address, "address are returned succesfully");
//         }
//         catch (ApplicationException ex)
//         {

//             return ApiResponse.ServerError("Server error: " + ex.Message);
//         }
//         catch (System.Exception ex)
//         {
//             return ApiResponse.ServerError("Server error: " + ex.Message);
//         }
//     }

//     [Authorize(Roles = "Admin,Customer")]
//     [HttpGet("{addressId}")]
//     public async Task<IActionResult> GetAddress(Guid addressId)
//     {
//         var address = await _addressService.GetAddressByIdServiceAsync(addressId);
//         if (address == null)
//         {
//             return ApiResponse.NotFound($"Address with this id {addressId} does not exist");
//         }
//         return ApiResponse.Success(address, "Address is returned succesfully");
//     }
//     [Authorize(Roles = "Customer")]
//     [HttpPut("{addressId}")]
//     public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] UpdateAddressDto updateAddressDto)
//     {
//         if (!ModelState.IsValid)
//         {
//             return ApiResponse.BadRequest("Invalid Adress Data");
//         }

//         var address = await _addressService.UpdateAddressByIdServiceAsync(addressId, updateAddressDto);
//         if (address == null)
//         {
//             return ApiResponse.BadRequest(" address nout found");

//         }
//         return ApiResponse.Success(address, "address is Updated succesfully");

//     }
//     [Authorize(Roles = "Customer")]
//     [HttpDelete("{addressId}")]

//     public async Task<IActionResult> DeleteAddress(Guid addressId)
//     {
//         var isDeleted = await _addressService.DeleteAddressByIdServiceAsync(addressId);
//         if (isDeleted == false)
//         {
//             return ApiResponse.NotFound($"address with this id {addressId} does not exist");
//         }
//         return ApiResponse.Success(isDeleted, "address is Deleted succesfully");
//     }


// }