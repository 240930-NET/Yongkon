// Profiles/ItemMappingProfile.cs
using AutoMapper;
using QuickOrder.API.DTO;
using QuickOrder.API.Model;

namespace QuickOrder.API;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Item, ItemDTO>().ReverseMap();;
        CreateMap<Item, UpdateItemDTO>().ReverseMap();;
        CreateMap<Order, OrderDTO>().ReverseMap();;
        CreateMap<Order, UpdateOrderDTO>().ReverseMap();;
        CreateMap<OrderItem, OrderItemDTO>().ReverseMap();;
    }

}