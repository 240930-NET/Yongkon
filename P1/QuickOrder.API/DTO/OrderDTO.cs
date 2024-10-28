using System.Text.Json.Serialization;

namespace QuickOrder.API.DTO;

public class OrderDTO
{
    public Boolean Completed {get; set;}

}


public class UpdateOrderDTO
{
    public int id {get; set;}
    public Boolean Completed {get; set;}

}

