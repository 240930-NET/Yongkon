using System.Text.Json.Serialization;

namespace QuickOrder.API.Model;

public class Order 
{
    public int Id {get; set;}

    public DateTime Date {get; set;} = DateTime.MinValue;
    
    public Boolean Completed {get; set;}

    [JsonIgnore]
    public ICollection<OrderItem> OrderItems {get; set;}  = new List<OrderItem>();
}