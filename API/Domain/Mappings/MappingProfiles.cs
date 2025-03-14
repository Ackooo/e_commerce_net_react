﻿namespace Domain.Mappings;

using AutoMapper;

using Domain.DTOs.Order;
using Domain.DTOs.Product;
using Domain.Entities.Order;
using Domain.Entities.Product;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<Order, OrderDto>();
    }
}