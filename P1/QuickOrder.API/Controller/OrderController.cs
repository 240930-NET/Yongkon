using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuickOrder.API.Model;
using QuickOrder.API.Service;

namespace QuickOrder.API.Controller;


[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IItemService _itemService;
    private readonly IMapper _imapper;

    public OrderController(IOrderService orderService, IItemService itemService, IMapper imapper) 
    {
         _orderService = orderService;
         _itemService = itemService;
         _imapper = imapper;
    }

    [HttpGet("/order")]
    public IActionResult GetAllOrders()
    {
        var orders = _orderService.GetAllOrders();
        return Ok(orders);
    }

    [HttpGet("/order/{id}")]
    public IActionResult GetOrderById(int id)
    {
        var order = _orderService.GetOrderById(id);
        return Ok(order);
    }

    [HttpGet("/orderWithItems/{orderId}")]
    public IActionResult GetAnOrderWithItems(int orderId)
    {
        var orderItems = _orderService.GetAnOrderWithItems(orderId);
        return Ok(orderItems);
    }

    [HttpGet("/order/item/{orderId}/{itemId}")]
    public IActionResult GetOrderItemById(int orderId, int itemId)
    {
        var orderItem = _orderService.GetOrderItemById(orderId, itemId);
        return Ok(orderItem);
    }
  
    // Order Create
    [HttpPost("/order/addNewOrder")]
    public IActionResult AddOrder([FromBody] OrderDTO orderDTO){

        var order = _imapper.Map<Order>(orderDTO);
        try{
            _orderService.AddOrder(order);
            return Ok(order);
        }
        catch(Exception e){
            return BadRequest("Could not add new order " +e.Message);
        }
    }

    [HttpPut("/order/updateOrder")]
    public IActionResult UpdateOrder([FromBody] UpdateOrderDTO updateOrderDTO){
        var order = _imapper.Map<Order>(updateOrderDTO);
        try{
            _orderService.UpdateOrder(order);
            return Ok(order);
        }
        catch(Exception e){
            return BadRequest("Could not update order. " + e.Message);
        }
    }

    [HttpDelete("/order/deleteOrder/{id}")]
    public IActionResult DeleteOrder(int id){

        try{
            Order searchedOrder = _orderService.GetOrderById(id);
            _orderService.DeleteOrder(searchedOrder.Id);
            return Ok($"Order Id: {searchedOrder.Id} | Date: {searchedOrder.Date} deleted");
        }
        catch(Exception e){
            return BadRequest(e.Message);
        }
    }     


    // Item Add to Order
    [HttpPost("/order/item/addNewItem")]
    public IActionResult AddItemToOrder([FromBody] OrderItem orderItem){

        try{
            _orderService.AddItemToOrder(orderItem);
            return Ok(orderItem);
        }
        catch(Exception e){
            return BadRequest(e.Message);
        }
    }

    [HttpPut("/order/item/updateItem")]
    public IActionResult UpdateItemAtOrder([FromBody] OrderItem orderItem){
        try{
            _orderService.UpdateItemAtOrder(orderItem);
            return Ok(orderItem);
        }
        catch(Exception e){
            return BadRequest("Could not update item at order. " + e.Message);
        }
    }

    [HttpDelete("/order/item/deleteItem/{orderId}/{itemId}")]
    public IActionResult DeleteItemAtOrder(int orderId, int itemId){

        try{
            OrderItem searchedOrderItem = _orderService.GetOrderItemById(orderId, itemId);
            _orderService.DeleteItemAtOrder(searchedOrderItem.OrderId, searchedOrderItem.ItemId);
            return Ok($"Order Id: {searchedOrderItem.OrderId} | Item: {_itemService.GetItemById(searchedOrderItem.ItemId).Name} deleted");
        }
        catch(Exception e){
            return BadRequest(e.Message);
        }
    }    
}