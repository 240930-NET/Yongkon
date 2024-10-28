// Profiles/ItemMappingProfile.cs
using AutoMapper;
using QuickOrder.API.DTO;
using QuickOrder.API.Model;

namespace QuickOrder.API;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Item
        CreateMap<Item, ItemDTO>().ReverseMap();
        CreateMap<Item, UpdateItemDTO>().ReverseMap();

        // Order
        CreateMap<Order, OrderDTO>().ReverseMap();
        CreateMap<Order, UpdateOrderDTO>().ReverseMap();

        // Order Items
        CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        

        CreateMap<IEnumerable<OrderItem>, OrderWithItemDTO>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Select(oi => new OrderItemDTO
            {
                OrderId = oi.OrderId,
                ItemId = oi.ItemId,
                Quantity = oi.Quantity
            })));        
    }

}