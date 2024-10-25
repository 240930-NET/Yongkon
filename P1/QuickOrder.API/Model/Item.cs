using System.Text.Json.Serialization;

namespace QuickOrder.API.Model;

public class Item {
    public int Id {get; set;}

    public string Name {get; set;} = "";

    public double Price {get; set;}

    [JsonIgnore]
    public ICollection<OrderItem> OrderItems {get; set;}  = new List<OrderItem>();
}