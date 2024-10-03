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
            
        }
    }