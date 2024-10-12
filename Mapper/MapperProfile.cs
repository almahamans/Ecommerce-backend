using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile(){
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();

        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, CategoryWithProductsDto>()
       .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products)); // to avoid null values and associate category with products.

        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderDto, PaginatedResult<OrderDto>>().ReverseMap();
        CreateMap<OrderCreateDto, Order>();
        CreateMap<OrderUpdateDto, Order>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); //onlu non-null values mapp to OrderDto 

        CreateMap<ShipmentStatus, Order>().ReverseMap();
        CreateMap<ShipmentStatus, OrderDto>().ReverseMap();
        CreateMap<Shipment, Order>().ReverseMap();
        CreateMap<Shipment, OrderDto>().ReverseMap();
        CreateMap<ShipmentUpdateDto, Shipment>();
        
        CreateMap<Shipment, ShipmentDto>();
        CreateMap<ShipmentUpdateDto, Shipment>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Shipment, ShipmentDto>().ReverseMap();

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<CreateUserDto, User>();

        //  CreateMap<Address, AddressDto>();
        // CreateMap<AddressDto, Address>();
        // CreateMap<CreateAddressDto, Address>();
    }
}
       
    


