using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Configuration.Annotations;

public class CategoryService
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
    public async Task<List<CategoryDto>> GetCategoryServiceAsync()
    {
        try
        {

            var categories = await _appDbContext.Categories.ToListAsync();
            var categoriesData = _mapper.Map<List<CategoryDto>>(categories);
            return categoriesData;
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

    public async Task<CategoryDto> GetCategoryByIdServiceAsync(Guid categoryId)
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
}