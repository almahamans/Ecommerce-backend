using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


public interface ICategoryService
{
    Task<Category> CreateCategoryServiceAsync(CreateCategoryDto newCategory);
    Task<PaginatedResult<CategoryDto>> GetCategoriesAsync(QueryParameters queryParameters);
    Task<CategoryDto?> GetCategoryByIdServiceAsync(Guid categoryId);
    Task<CategoryDto?> UpdateCategoryServiceAsync(UpdateCategoryDto updateCategory, Guid categoryId);
    Task<bool> DeleteCategoryByIdServiceAsync(Guid categoryId);
    Task<PaginatedResult<CategoryWithProductsDto>> GetCategoriesWithProductsAsync(int pageNumber, int pageSize);
    Task<List<CategoryWithProductsDto?>> GetProductsByCategoryIdServiceAsync(Guid categoryId);
}

public class CategoryService : ICategoryService
{

    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CategoryService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    // create category service
    public async Task<Category> CreateCategoryServiceAsync(CreateCategoryDto newCategory)
    {

        try
        {

            var slug = newCategory.CategoryName.Replace(" ", "-");
            newCategory.Slug = slug;
            var category = _mapper.Map<Category>(newCategory);
            await _appDbContext.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return category;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions (like unique constraint violations)
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }

    }
    public async Task<PaginatedResult<CategoryDto>> GetCategoriesAsync(QueryParameters queryParameters)
    {
        try
        {
            var query = _appDbContext.Categories.AsQueryable();

            //searching by category name (if needed)
            if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                query = query.Where(c => c.CategoryName.Contains(queryParameters.SearchTerm));
            }

            var totalCategories = await query.CountAsync(); // Total count of categories

            // Sort by the specified property in descending order
            if (!string.IsNullOrWhiteSpace(queryParameters.SortBy))
            {
                var sortProperties = queryParameters.SortBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var property in sortProperties)
                {
                    query = query.OrderByDescending(c => EF.Property<object>(c, property.Trim()));
                }
            }
            else
            {
                query = query.OrderByDescending(c => c.CreatedAt); // Default sorting if no SortBy is provided
            }

            var categories = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync(); // Use ToListAsync for asynchronous operation

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            return new PaginatedResult<CategoryDto>
            {
                Items = categoryDtos,
                TotalCount = totalCategories,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<CategoryDto?> GetCategoryByIdServiceAsync(Guid categoryId)
    {
        try
        {
            var category = await _appDbContext.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return null;
            }
            var categoryDate = _mapper.Map<CategoryDto>(category);
            return categoryDate;
        }catch (Exception ex){
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }
    public async Task<List<CategoryWithProductsDto>> GetProductsByCategoryIdServiceAsync(Guid categoryId)
    {
            var products = await _appDbContext.Products
                                   .Where(x => x.CategoryId == categoryId)
                                   .ToListAsync();
            if (products == null || !products.Any())  return [];
            if (categoryId == Guid.Empty) throw new ArgumentException("Invalid category ID.");

          return  new List<CategoryWithProductsDto>{
            new CategoryWithProductsDto {
                CategoryId = categoryId,
                CategoryName = "Sample Category",
                Slug = "sample-category",          
                CreatedAt = DateTime.UtcNow,      
                Products = products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,    
                    ProductName = p.ProductName,           
                    Price = p.Price,
                    Description = p.Description,
                    Image = p.Image
                }).ToList()
             }  
            };      
    }

    public async Task<CategoryDto?> UpdateCategoryServiceAsync(UpdateCategoryDto updateCategory, Guid categoryId)
    {
        try
        {
            Console.WriteLine($"---test1----");
            var category = await _appDbContext.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return null; // Category not found
            }

            Console.WriteLine($"---test2----");
            category.CategoryName = updateCategory.CategoryName ?? category.CategoryName;

            Console.WriteLine($"---test3----");
            _appDbContext.Update(category);

            Console.WriteLine($"---test4----");
            await _appDbContext.SaveChangesAsync();

            Console.WriteLine($"---test5----");
            var categoryData = _mapper.Map<CategoryDto>(category);
            return categoryData;
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }


    }


    public async Task<bool> DeleteCategoryByIdServiceAsync(Guid categoryId)
    {

        try
        {
            var category = await _appDbContext.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return false;
            }
            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();
            return true;

        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions (like unique constraint violations)
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }



    }
    public async Task<PaginatedResult<CategoryWithProductsDto>> GetCategoriesWithProductsAsync(int pageNumber, int pageSize)
    {
        var totalCategories = await _appDbContext.Categories.CountAsync(); // Total count of categories

        var categories = await _appDbContext.Categories
            .Include(c => c.Products) // Eager load products
            .Skip((pageNumber - 1) * pageSize) // Skip for pagination
            .Take(pageSize) // Take the number of records for the current page
            .ToListAsync(); // Use ToListAsync for asynchronous operation

        // Map to DTOs
        var categoryWithProductsDtos = _mapper.Map<List<CategoryWithProductsDto>>(categories);

        return new PaginatedResult<CategoryWithProductsDto>
        {
            Items = categoryWithProductsDtos,
            TotalCount = totalCategories,
            PageNumber = pageNumber,
            PageSize = pageSize


        };
    }


}