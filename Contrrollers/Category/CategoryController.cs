using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categories")]
#pragma warning disable CA1050 // Declare types in namespaces
public class CategoryController : ControllerBase
#pragma warning restore CA1050 // Declare types in namespaces
{

    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto newCategory)
    {

        if (!ModelState.IsValid)
        {
            return ApiResponse.BadRequest("Invalid category Data");
        }
        try
        {
            var category = await _categoryService.CreateCategoryServiceAsync(newCategory);
            return ApiResponse.Created(category, "Category is created");
        }
        catch (ApplicationException ex)
        {
            return ApiResponse.ServerError("Server error: CreateCategory" + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: CreateCategory" + ex.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Customer")]
    public async Task<IActionResult> GetCategories([FromQuery] QueryParameters queryParameters)
    {
        try
        {
            var categories = await _categoryService.GetCategoriesAsync(queryParameters);
            return ApiResponse.Success(categories, "Categories are returned successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponse.ServerError("Server error: GetCategories - " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: GetCategories - " + ex.Message);
        }
    }

    [HttpGet("{categoryId}")]
    [Authorize(Roles = "Admin, Customer")]
    public async Task<IActionResult> GetCategoryById(Guid categoryId)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdServiceAsync(categoryId);
            if (category == null)
            {
                return ApiResponse.NotFound($"Category with this id {categoryId} does not exist");
            }
            return ApiResponse.Success(category, "Category is returned successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponse.ServerError("Server error: GetCategoryById " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: GetCategoryById " + ex.Message);
        }
    }
    [HttpPut("{categoryId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategory, Guid categoryId)
    {


        if (!ModelState.IsValid)
        {
            return ApiResponse.BadRequest("Invalid category Data");
        }
        try
        {
            var category = await _categoryService.UpdateCategoryServiceAsync(updateCategory, categoryId);
            if (category == null)
            {
                return ApiResponse.NotFound($"Updated categorydata cannot be null");
            }
            return ApiResponse.Success(category, "category is updated");
        }
        catch (ApplicationException ex)
        {
            return ApiResponse.ServerError("Server error: UpdateCategory " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: UpdateCategory" + ex.Message);
        }

    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeletCategory(Guid categoryId)
    {


        if (!ModelState.IsValid)
        {
            return ApiResponse.BadRequest("Invalid category Data");
        }
        try
        {
            var isExist = await _categoryService.DeleteCategoryByIdServiceAsync(categoryId);
            if (isExist == false)
            {
                return ApiResponse.NotFound($"Deleting the category failed, Category with this id {categoryId} does not exist");
            }
            return ApiResponse.Success(isExist, "category is deleted");
        }
        catch (ApplicationException ex)
        {
            return ApiResponse.ServerError("Server error: DeletCategory " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: DeletCategory " + ex.Message);
        }

    }

    [HttpGet("products")]
    [Authorize(Roles = "Admin, Customer")]
    public async Task<IActionResult> GetCategoriesWithProducts(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var pagedResult = await _categoryService.GetCategoriesWithProductsAsync(pageNumber, pageSize);
            return Ok(ApiResponse.Success(pagedResult, "Categories with products returned successfully."));
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ApiResponse.ServerError("Server error: GetCategoriesWithProducts " + ex.Message));
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, ApiResponse.ServerError("Server error: GetCategoriesWithProducts" + ex.Message));
        }
    }

    [HttpGet("products/{categoryId}")]
    [Authorize(Roles = "Admin, Customer")]
    public async Task<IActionResult> GetProductsByCategoryId(Guid categoryId)
    {
        try
        {
            var pagedResult = await _categoryService.GetProductsByCategoryIdServiceAsync(categoryId);
            return Ok(ApiResponse.Success(pagedResult, "products with category returned successfully."));
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ApiResponse.ServerError("Server error: GetProductsByCategoryIdServiceAsync " + ex.Message));
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, ApiResponse.ServerError("Server error: GetProductsByCategoryIdServiceAsync" + ex.Message));
        }
    }


}