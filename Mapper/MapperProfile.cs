using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, OrderDto>();

        CreateMap<CreateShipmentDto, Shipment>();
        CreateMap<UpdateShipmentDto, ShipmentDto>();
        CreateMap<Shipment, ShipmentDto>().ReverseMap();

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<CreateUserDto, User>();
    }
}

