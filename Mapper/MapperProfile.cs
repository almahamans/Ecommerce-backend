using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
// using ecommerce_db_api.EFCore;
// using ecommerce_db_api.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();
        }
    }