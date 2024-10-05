using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;


public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();

        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, CategoryWithProductsDto>()
       .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products)); // to avoid null values and associate category with products.

    }
}