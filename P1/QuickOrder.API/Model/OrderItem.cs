using System.Text.Json.Serialization;

namespace QuickOrder.API.Model;

public class OrderItem
{
    public int OrderId {get; set;}

    public Order? Order {get; set;} 

    public int ItemId {get; set;}

    public Item? Item {get; set;}

    public int Quantity {get; set;}
}