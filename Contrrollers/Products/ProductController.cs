// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("api/products")]

// public class ProductController : ControllerBase
// {
//     // get products
//     [HttpGet]
//     public async Task<IActionResult> GetProducts()
//     {

//         try
//         {
//             var users = await _userService.GetUsersServiceAsync();
//             return ApiResponse.Success(users, "Users are returned succesfully");
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
//     // create products
//     // update products
//     // delete products

// }