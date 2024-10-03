using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            var product = await _productService.CreateProductServiceAsync(newProduct);
            return ApiResponse.Created(product, "Product is created");
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
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productService.GetProductsServiceAsync();

            return ApiResponse.Success(products, "Products are returned succesfully");
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

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductById(Guid productId)
    {
        try
        {
            var product = await _productService.GetProductByIdServiceAsync(productId);
            if (product == null)
            {
                return ApiResponse.NotFound($"Product with this id {productId} does not exist");
            }
            return ApiResponse.Success(product, "Product is returned successfully");
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

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProductServiece(UpdateProductDto updateProduct, Guid productId)
    {


        if (!ModelState.IsValid)
        {
            return ApiResponse.BadRequest("Invalid product Data");
        }
        try
        {
            var product = await _productService.UpdateProductServiceAsync(updateProduct, productId);
            if (product == null)
            {
                return ApiResponse.NotFound($"Updated Product data cannot be null");
            }
            return ApiResponse.Success(product, "product is updated");
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

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeletProductServiece(Guid productId)
    {


        if (!ModelState.IsValid)
        {
            return ApiResponse.BadRequest("Invalid product Data");
        }
        try
        {
            var isExist = await _productService.DeleteProductByIdServiceAsync(productId);
            if (isExist == false)
            {
                return ApiResponse.NotFound($"Deleting the product failed, Product with this id {productId} does not exist");
            }
            return ApiResponse.Success(isExist, "product is deleted");
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



}