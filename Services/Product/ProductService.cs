using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
public class ProductService
{

    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }


    public async Task<Product> CreateProductServieceAsync(CreateProductDto newProduct)
    {

        try
        {
            var product = _mapper.Map<Product>(newProduct);
            await _appDbContext.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return product;
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