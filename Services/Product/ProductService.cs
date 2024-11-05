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
    // // get products and the category associated with it
    // public async Task<PaginatedResult<ProductDto>> GetProductsServiceAsync(QueryParameters queryParameters)
    // {
    //     try{
    //         var query = _appDbContext.Products.AsQueryable();
    //         // If searchTerm is provided and not empty, apply filtering
    //         if (!string.IsNullOrWhiteSpace(queryParameters.searchTerm)){
    //             // Convert searchTerm to lowercase for case-insensitive search
    //             var searchTermLower = queryParameters.searchTerm.ToLower();
    //             // Apply case-insensitive search for both ProductName and Description
    //             query = query.Where(p => p.ProductName.ToLower().Contains(searchTermLower) ||
    //                                       p.Description.ToLower().Contains(searchTermLower));
    //             Console.WriteLine("Filtering applied based on searchTerm.");
    //         }else{
    //             // If searchTerm is empty or null, show all products
    //             Console.WriteLine("No searchTerm provided, returning all products.");
    //         }
    //         var totalProducts = await query.CountAsync();
    //         // Determine sorting order based on SortOrder parameter
    //         // if (!string.IsNullOrWhiteSpace(queryParameters.sortBy)){
    //         //     if (queryParameters.sortOrder?.ToLower() == "asc"){
    //         //         query = query.OrderBy(p => EF.Property<object>(p, queryParameters.sortBy));
    //         //     }else{
    //         //         query = query.OrderByDescending(p => EF.Property<object>(p, queryParameters.sortBy));
    //         //     }
    //         // }else{
    //         //     query = query.OrderBy(p => p.CreatedAt); // Default sorting if no SortBy is provided
    //         // }
    //         // // Sort by the specified property in descending order
    //         // if (!string.IsNullOrWhiteSpace(queryParameters.sortBy)){
    //         //     query = query.OrderByDescending(p => EF.Property<object>(p, queryParameters.sortBy));
    //         // }else{
    //         //     query = query.OrderByDescending(p => p.CreatedAt); // Default sorting if no SortBy is provided
    //         // }

    //         if (!string.IsNullOrWhiteSpace(queryParameters.sortBy))
    //         {
    //             // Check the sort order and apply accordingly
    //             if (queryParameters.sortOrder?.ToLower() == "asc")
    //             {
    //                 query = query.OrderBy(p => EF.Property<object>(p, queryParameters.sortBy));
    //             }
    //             else
    //             {
    //                 query = query.OrderByDescending(p => EF.Property<object>(p, queryParameters.sortBy));
    //             }
    //         }
    //         else
    //         {
    //             // Default sorting by CreatedAt if no sortBy is provided
    //             query = query.OrderBy(p => p.CreatedAt);
    //         }


    //         var products = await query
    //             .Include(p => p.Category)
    //             .Skip((queryParameters.pageNumber - 1) * queryParameters.pageSize)
    //             .Take(queryParameters.pageSize)
    //             .ToListAsync();

    //         var productsData = _mapper.Map<List<ProductDto>>(products);

    //         return new PaginatedResult<ProductDto>
    //         {
    //             Items = productsData,
    //             TotalCount = totalProducts,
    //             PageNumber = queryParameters.pageNumber,
    //             PageSize = queryParameters.pageSize
    //         };
    //     }
    //     catch (DbUpdateException dbEx)
    //     {
    //         Console.WriteLine($"Database Update Error: {dbEx.Message}");
    //         throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"An unexpected error occurred: {ex.Message}");
    //         throw new ApplicationException("An unexpected error occurred. Please try again later.");
    //     }
    // }

    public async Task<PaginatedResult<ProductDto>> GetProductsServiceAsync(QueryParameters queryParameters)
    {
        try
        {
            var query = _appDbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters.searchTerm))
            {
                var searchTermLower = queryParameters.searchTerm.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(searchTermLower) ||
                                         p.Description.ToLower().Contains(searchTermLower));
                Console.WriteLine("Filtering applied based on searchTerm.");
            }

            var totalProducts = await query.CountAsync();

            // Apply sorting based on sortBy and sortOrder
            if (!string.IsNullOrWhiteSpace(queryParameters.sortBy))
            {
                if (queryParameters.sortOrder?.ToLower() == "asc")
                {
                    query = query.OrderBy(p => EF.Property<object>(p, queryParameters.sortBy));
                }
                else
                {
                    query = query.OrderByDescending(p => EF.Property<object>(p, queryParameters.sortBy));
                }
            }
            else
            {
                query = query.OrderBy(p => p.CreatedAt);
            }

            var products = await query
                .Include(p => p.Category)
                .Skip((queryParameters.pageNumber - 1) * queryParameters.pageSize)
                .Take(queryParameters.pageSize)
                .ToListAsync();

            var productsData = _mapper.Map<List<ProductDto>>(products);

            return new PaginatedResult<ProductDto>
            {
                Items = productsData,
                TotalCount = totalProducts,
                PageNumber = queryParameters.pageNumber,
                PageSize = queryParameters.pageSize
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