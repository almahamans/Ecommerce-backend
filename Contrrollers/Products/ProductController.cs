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

    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }



    // create products
    [HttpPost]
    // [Authorize("Admin")]
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
            return ApiResponse.ServerError("Server error: CreateProduct" + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: CreateProduct" + ex.Message);
        }
    }

    [HttpGet]
    // [Authorize("Admin, Customer")]
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] QueryParameters queryParameters)
    {
        try
        {
            var products = await _productService.GetProductsServiceAsync(queryParameters);

            return ApiResponse.Success(products, "Products are returned successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponse.ServerError("Server error: GetProducts - " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: GetProducts - " + ex.Message);
        }
    }

    [HttpGet("{productId}")]
    // [Authorize("Admin, Customer")]
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
            return ApiResponse.ServerError("Server error: GetProductById" + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: GetProductById" + ex.Message);
        }
    }

    [HttpPut("{productId}")]
    // [Authorize("Admin")]
    public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProduct, Guid productId)
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
            return ApiResponse.ServerError("Server error: UpdateProduct" + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: UpdateProduct" + ex.Message);
        }

    }

    [HttpDelete("{productId}")]
    // [Authorize("Admin")]
    public async Task<IActionResult> DeletProduct(Guid productId)
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
            return ApiResponse.ServerError("Server error: DeletProduct" + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: DeletProduct" + ex.Message);
        }

    }



}