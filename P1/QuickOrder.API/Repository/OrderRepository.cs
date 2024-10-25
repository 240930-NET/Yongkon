using Microsoft.EntityFrameworkCore;
using QuickOrder.API.Data;
using QuickOrder.API.Model;

namespace QuickOrder.API.Repository;

public class OrderRepository : IOrderRepository 
{
    private readonly OrderContext _orderContext;

    public OrderRepository(OrderContext orderContext) => _orderContext = orderContext;

    public IEnumerable<Order> GetAllOrders()
    {
        return _orderContext.Orders.ToList();
    }

    public Order GetOrderById(int id)
    {
        var order = _orderContext.Orders.FirstOrDefault(i => i.Id == id);
        if (order != null) {
            return order;
        } else {
            throw new Exception("Order not found");
        }

    }

    public IEnumerable<OrderItem> GetAnOrderWithItems(int orderId)
    {
        return _orderContext.OrderItems
          .Where(oi => oi.OrderId == orderId)
          .Include(oi => oi.Order)
          .Include(oi => oi.Item) 
          .ToList();
    }

    public OrderItem GetOrderItemById(int orderId, int itemId)
    {
        var item = _orderContext.OrderItems.FirstOrDefault(i => i.OrderId == orderId && i.ItemId == itemId);
        if (item != null) {
            return item;
        } else {
            throw new Exception("OrderItem not found");
        }
    }

    public void AddOrder(Order order)
    {
        _orderContext.Orders.Add(order);
        _orderContext.SaveChanges();
        
    }

    public void UpdateOrder(Order order)
    {
        _orderContext.Orders.Update(order);
        _orderContext.SaveChanges();
    }
    
    public void DeleteOrder(Order order)
    {
        var orderItems = _orderContext.OrderItems
                              .Where(oi => oi.OrderId == order.Id)
                              .ToList();

        _orderContext.OrderItems.RemoveRange(orderItems);
        _orderContext.Orders.Remove(order);        
        _orderContext.SaveChanges();
    }

    //Item to Order
    public void AddItemToOrder(OrderItem orderItem)
    {
        _orderContext.OrderItems.Add(orderItem);
        _orderContext.SaveChanges();
    }
    public void UpdateItemAtOrder(OrderItem orderItem)
    {
        _orderContext.OrderItems.Update(orderItem);
        _orderContext.SaveChanges();
    }
    public void DeleteItemAtOrder(OrderItem orderItem)
    {
        _orderContext.OrderItems.Remove(orderItem);
        _orderContext.SaveChanges();
    }
}