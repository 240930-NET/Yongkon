using System.Text.Json.Serialization;

namespace QuickOrder.API.Model;

public class Order 
{
    public int Id {get; set;}

    public DateTime Date {get; set;}
    
    public Boolean Completed {get; set;} = false;

    [JsonIgnore]
    public ICollection<OrderItem> OrderItems {get; set;}  = new List<OrderItem>();
}