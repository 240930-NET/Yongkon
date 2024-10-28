using System.Text.Json.Serialization;

namespace QuickOrder.API.DTO;

public class OrderItemDTO
{
    public int OrderId {get; set;}
    public int ItemId {get; set;}
    public int Quantity {get; set;}
}
public class DeleteItemDTO
{
    public int OrderId {get; set;}
    public int ItemId {get; set;}
}

public class OrderWithItemDTO
{
    public int OrderId { get; set; }
    public IEnumerable<OrderItemDTO>? OrderItems { get; set; } 
}