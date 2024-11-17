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
    Task<PaginatedResult<ProductDto>> GetProductsServiceAsync(QueryParameters queryParameters);
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
        try
        {
            var slug = newProduct.ProductName.Replace(" ", "-");
            newProduct.Slug = slug;
            var product = _mapper.Map<Product>(newProduct);
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

            foreach(var orderProducts in newProduct.OrderProducts){
                var newOrderProduct = new OrderProduct{
                    ProductQuantity = orderProducts.ProductQuantity,
                    ProductsPrice = orderProducts.ProductsPrice,
                    ProductId = orderProducts.ProductId
                };
                await _appDbContext.OrderProducts.AddAsync(newOrderProduct);
            }
            await _appDbContext.SaveChangesAsync();

            var productData = _mapper.Map<ProductDto>(product);
            return productData;
        }catch (DbUpdateException dbEx){
            // Handle database update exceptions (like unique constraint violations)
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }catch (Exception ex){
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }
 // get products and the category associated with it
    public async Task<PaginatedResult<ProductDto>> GetProductsServiceAsync(QueryParameters queryParameters)
    {
        try
        {
            var query = _appDbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                var searchTermLower = queryParameters.SearchTerm.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(searchTermLower) ||
                                         p.Description.ToLower().Contains(searchTermLower));
                Console.WriteLine("Filtering applied based on searchTerm.");
            }

            var totalProducts = await query.CountAsync();

            // Apply sorting based on sortBy and sortOrder
            if (!string.IsNullOrWhiteSpace(queryParameters.SortBy))
            {
                if (queryParameters.SortOrder?.ToLower() == "asc")
                {
                    query = query.OrderBy(p => EF.Property<object>(p, queryParameters.SortBy));
                }
                else
                {
                    query = query.OrderByDescending(p => EF.Property<object>(p, queryParameters.SortBy));
                }
            }
            else
            {
                query = query.OrderBy(p => p.CreatedAt);
            }

            var products = await query
                .Include(p => p.Category)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();

            var productsData = _mapper.Map<List<ProductDto>>(products);

            return new PaginatedResult<ProductDto>
            {
                Items = productsData,
                TotalCount = totalProducts,
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

    public async Task<ProductDto?> GetProductByIdServiceAsync(Guid productId)
    {
        try{
         var product = await _appDbContext.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == productId);
          if (product == null){
                return null;
            }
            var productDate = _mapper.Map<ProductDto>(product);
            return productDate;
        }catch (Exception ex){
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
        try{
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