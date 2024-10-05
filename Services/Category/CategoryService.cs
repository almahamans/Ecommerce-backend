using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Configuration.Annotations;


public interface ICategoryService
{
    Task<Category> CreateCategoryServiceAsync(CreateCategoryDto newCategory);
    Task<PagedResult<CategoryDto>> GetCategoriesAsync(int pageNumber, int pageSize);
    Task<CategoryDto?> GetCategoryByIdServiceAsync(Guid categoryId);
    Task<CategoryDto?> UpdateCategoryServiceAsync(UpdateCategoryDto updateCategory, Guid categoryId);
    Task<bool> DeleteCategoryByIdServiceAsync(Guid categoryId);
    Task<PagedResult<CategoryWithProductsDto>> GetCategoriesWithProductsAsync(int pageNumber, int pageSize);
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

        Console.WriteLine($"-------Test1----------");

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
    public async Task<PagedResult<CategoryDto>> GetCategoriesAsync(int pageNumber, int pageSize)
    {
        try
        {
            var totalCategories = await _appDbContext.Categories.CountAsync(); // Total count of categories

            var categories = await _appDbContext.Categories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize) // Take the number of records for the current page
                .ToListAsync(); // Use ToListAsync for asynchronous operation

            // Map to DTOs
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            return new PagedResult<CategoryDto>
            {
                TotalCount = totalCategories,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Items = categoryDtos
            };
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
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
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }


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
    public async Task<PagedResult<CategoryWithProductsDto>> GetCategoriesWithProductsAsync(int pageNumber, int pageSize)
    {
        var totalCategories = await _appDbContext.Categories.CountAsync(); // Total count of categories

        var categories = await _appDbContext.Categories
            .Include(c => c.Products) // Eager load products
            .Skip((pageNumber - 1) * pageSize) // Skip for pagination
            .Take(pageSize) // Take the number of records for the current page
            .ToListAsync(); // Use ToListAsync for asynchronous operation

        // Map to DTOs
        var categoryWithProductsDtos = _mapper.Map<List<CategoryWithProductsDto>>(categories);

        return new PagedResult<CategoryWithProductsDto>
        {
            TotalCount = totalCategories,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            Items = categoryWithProductsDtos
        };
    }


}