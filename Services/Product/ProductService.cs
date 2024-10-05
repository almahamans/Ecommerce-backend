using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Configuration.Annotations;

public interface IProductService
{
    Task<ProductDto> CreateProductServiceAsync(CreateProductDto newProduct);
    Task<PagedResult<ProductDto>> GetProductsServiceAsync(int pageNumber, int pageSize);
    Task<ProductDto?> GetProductByIdServiceAsync(Guid productId);
    Task<ProductDto?> UpdateProductServiceAsync(UpdateProductDto updateProduct, Guid productId);
    Task<bool> DeleteProductByIdServiceAsync(Guid productId);
}
public class ProductService : IProductService
{

    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    // create product service
    public async Task<ProductDto> CreateProductServiceAsync(CreateProductDto newProduct)
    {

        Console.WriteLine($"-------Test1----------");

        try
        {
            var slug = newProduct.ProductName.Replace(" ", "-");
            newProduct.Slug = slug;
            var product = _mapper.Map<Product>(newProduct);
            Console.WriteLine($"-------Test2----------");
            await _appDbContext.Products.AddAsync(product);
            Console.WriteLine($"-------Test3----------");
            await _appDbContext.SaveChangesAsync();
            Console.WriteLine($"-------Test4----------");
            //revearce the produc befot returnng it
            var productData = _mapper.Map<ProductDto>(product);
            return productData;
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

    // get products and the category associated with it
    public async Task<PagedResult<ProductDto>> GetProductsServiceAsync(int pageNumber, int pageSize)
    {
        try
        {
            // Get the total count of products
            var totalProducts = await _appDbContext.Products.CountAsync();

            // Get the products from the database, sorted by created date and paginated
            var products = await _appDbContext.Products
                .Include(p => p.Category) // Eager load the Category
                .OrderByDescending(p => p.CreatedAt) // Sort by created date, descending
                .Skip((pageNumber - 1) * pageSize) // Skip for pagination
                .Take(pageSize) // Take the specified page size
                .ToListAsync(); // Use ToListAsync for asynchronous operation

            // Convert products to ProductDto
            var productsData = _mapper.Map<List<ProductDto>>(products);

            return new PagedResult<ProductDto>
            {
                TotalCount = totalProducts,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Items = productsData
            };
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

    public async Task<ProductDto?> GetProductByIdServiceAsync(Guid productId)
    {
        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }
            var productDate = _mapper.Map<ProductDto>(product);
            return productDate;
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }


    }

    public async Task<ProductDto?> UpdateProductServiceAsync(UpdateProductDto updateProduct, Guid productId)
    {
        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }

            product.ProductName = updateProduct.ProductName ?? product.ProductName;
            product.Price = updateProduct.Price ?? product.Price;
            product.Description = updateProduct.Description ?? product.Description;
            product.Quantity = updateProduct.Quantity ?? product.Quantity;
            product.Image = updateProduct.Image ?? product.Image;
            Console.WriteLine($"---test3----");
            _appDbContext.Update(product);
            await _appDbContext.SaveChangesAsync();
            var productDate = _mapper.Map<ProductDto>(product);
            return productDate;
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }


    }


    public async Task<bool> DeleteProductByIdServiceAsync(Guid productId)
    {

        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }
            _appDbContext.Products.Remove(product);
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