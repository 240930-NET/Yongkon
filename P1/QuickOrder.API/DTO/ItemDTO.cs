using System.Text.Json.Serialization;

namespace QuickOrder.API.DTO;

public class ItemDTO {
    
    public required string Name {get; set;}

    public double Price {get; set;}

}

public class UpdateItemDTO {
    public int Id {get; set;}

    public required string  Name {get; set;}

    public double Price {get; set;}
}