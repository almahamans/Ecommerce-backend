using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{

    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

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
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(int pageNumber = 1, int pageSize = 5)
    {
        try
        {
            var categories = await _categoryService.GetCategoriesAsync(pageNumber, pageSize);
            return ApiResponse.Success(categories, "Categories are returned succesfully");
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

    [HttpGet("{categoryId}")]
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
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }
    }
    [HttpPut("{categoryId}")]
    public async Task<IActionResult> UpdateCategoryServiece(UpdateCategoryDto updateCategory, Guid categoryId)
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
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }

    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeletCategoryServiece(Guid categoryId)
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
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponse.ServerError("Server error: " + ex.Message);
        }

    }

    [HttpGet("products")]
    public async Task<IActionResult> GetCategoriesWithProducts(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var pagedResult = await _categoryService.GetCategoriesWithProductsAsync(pageNumber, pageSize);
            return Ok(ApiResponse.Success(pagedResult, "Categories with products returned successfully."));
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ApiResponse.ServerError("Server error: " + ex.Message));
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, ApiResponse.ServerError("Server error: " + ex.Message));
        }
    }


}