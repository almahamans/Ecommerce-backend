using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using ecommerce_db_api.Utilities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]

public class ProductController : ControllerBase
{

    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    // create products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto newProduct)
    {

        if (!ModelState.IsValid)
        {
            return ApiResponse.BadRequest("Invalid Product Data");
        }
        try
        {
            var user = await _productService.CreateProductServieceAsync(newProduct);
            return ApiResponse.Created(user, "Product is created");
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


    // get products
    // [HttpGet]
    // public async Task<IActionResult> GetProducts()
    // {

    //     try
    //     {
    //         var users = await _userService.GetUsersServiceAsync();
    //         return ApiResponse.Success(users, "Users are returned succesfully");
    //     }
    //     catch (ApplicationException ex)
    //     {
    //         return ApiResponse.ServerError("Server error: " + ex.Message);
    //     }
    //     catch (System.Exception ex)
    //     {
    //         return ApiResponse.ServerError("Server error: " + ex.Message);
    //     }


    // }

    // update products
    // delete products

}