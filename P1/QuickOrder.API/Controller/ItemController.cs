using Microsoft.AspNetCore.Mvc;
using QuickOrder.API.DTO;
using QuickOrder.API.Model;
using QuickOrder.API.Service;

namespace QuickOrder.API.Controller;


[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService) => _itemService = itemService;

    [HttpGet("/item")]
    public IActionResult GetAllItems()
    {
        try {
            var items = _itemService.GetAllItems();
            return Ok(items);
        } catch (Exception e) {
            return StatusCode(500, e.Message);  // return server error with the error message
        }        
    }

    [HttpGet("/item/{id}")]
    public IActionResult GetItemById(int id)
    {
        try {
            var item = _itemService.GetItemById(id);
            return Ok(item);
        
        } catch (Exception e) {
            return StatusCode(500, e.Message);  // return server error with the error message
        }        
    }

  [HttpPost("/item/addNewItem")]
    public IActionResult AddNewItem([FromBody] ItemDTO itemDTO){
        
        try{
            _itemService.AddItem(itemDTO);
            return Ok(itemDTO);
        }
        catch(Exception e){
            return BadRequest("Could not add item " + e.Message);
        }
    }

    [HttpPut("/itme/updateItem")]
    public IActionResult EditItem([FromBody] UpdateItemDTO updateItemDTO){
        try{
            _itemService.UpdateItem(updateItemDTO);
            return Ok(updateItemDTO);
        }
        catch(Exception e){
            return BadRequest("Could not update item "+ e.Message);
        }
    }

    [HttpDelete("/item/deleteItem/{id}")]
    public IActionResult DeleteItem(int id){

        try{
            Item searchedItem = _itemService.GetItemById(id);
            _itemService.DeleteItem(searchedItem.Id);
            return Ok($"Item {searchedItem.Name} deleted");
        }
        catch(Exception e){
            return BadRequest("Could not delete item " + e.Message);
        }
    }    
}