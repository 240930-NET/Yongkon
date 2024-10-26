using Microsoft.EntityFrameworkCore;
using QuickOrder.API.Data;
using QuickOrder.API.Model;
using QuickOrder.API.Repository;

namespace QuickOrder.API.Service;

public class OrderService : IOrderService 
{
    private readonly IOrderRepository _orderRepository;
    private readonly IItemRepository _itemRepository;

    private readonly OrderContext _context;
    public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, OrderContext context) 
    {
        _orderRepository = orderRepository;
        _itemRepository = itemRepository;
        _context = context;
    } 

    public IEnumerable<Order> GetAllOrders() 
    {
        IEnumerable<Order> orders = _orderRepository.GetAllOrders();
        if (orders.Count() == 0) {
            throw new Exception("No Items were found");
        } else {
            return orders;
        }             
    }

    public Order GetOrderById(int id) 
    {
        return _orderRepository.GetOrderById(id);
    }

    public IEnumerable<OrderItem> GetAnOrderWithItems(int orderId) 
    {
        return _orderRepository.GetAnOrderWithItems(orderId);
    }

    public OrderItem GetOrderItemById(int orderId, int itemId)
    {
        return _orderRepository.GetOrderItemById(orderId, itemId);
    }

    // Order
    public string AddOrder(Order order) 
    {
        if (!order.Completed) 
        {
            _orderRepository.AddOrder(order);
            return $"Order {order.Id} added successfully!";
        } else {
            throw new Exception("Invalid order. Please check the date or status information.");
        }
    }

    public Order UpdateOrder(Order order)
    {
        var searchedOrder = _orderRepository.GetOrderById(order.Id);
        if (searchedOrder != null) 
        {
            if (!searchedOrder.Completed)
            {
                // Update order as completed
                searchedOrder.Completed = order.Completed;

                _orderRepository.UpdateOrder(searchedOrder);
                return searchedOrder;
            } else if (searchedOrder.Completed) {
                throw new Exception("Completed order can not be changed.");
            } else {                            
                throw new Exception("Invalid Order! Please check date or order id");
            }
        } else {
            throw new Exception("Invalid Order! Does not exist.");
        }
    }

    public string DeleteOrder(int id)
    {
        Order searchedOrder = _orderRepository.GetOrderById(id);

        if (searchedOrder != null)
        { 
            if (!searchedOrder.Completed) {
                //Delete
                _orderRepository.DeleteOrder(searchedOrder);
                return $"Order Id:{searchedOrder.Id} | Date:{searchedOrder.Date} is deleted successfully.";
            } else {
                throw new Exception("Completed order can not be deleted.");
            }
        } else {
            throw new Exception("Invalid Order! Does not exist.");
        }
    }

   // OrderItem
    public string AddItemToOrder(OrderItem orderItem) 
    {
        Order searchedOrder = _orderRepository.GetOrderById(orderItem.OrderId);
        Item searchedItem = _itemRepository.GetItemById(orderItem.ItemId);

        if (!searchedOrder.Completed && orderItem.Quantity > 0) 
        {   
            orderItem.Order = searchedOrder;
            orderItem.Item = searchedItem;
            _orderRepository.AddItemToOrder(orderItem);
            return $"Item {searchedItem.Name}  {orderItem.Quantity} ea. added successfully!";

        } else if(searchedOrder.Completed) {
            throw new Exception("Order completed. New item could not be added.");
        } else if(orderItem.Quantity <= 0) {
            throw new Exception("Quantity cannot be less than one!");
        } else {
            throw new Exception("Invalid Item.");
        }
    }

    public OrderItem UpdateItemAtOrder(OrderItem orderItem)
    {
        Order searchedOrder = _orderRepository.GetOrderById(orderItem.OrderId);
        Item searchedItem = _itemRepository.GetItemById(orderItem.ItemId);
        OrderItem searchedOrderItem = _orderRepository.GetOrderItemById(orderItem.OrderId, orderItem.ItemId);
        //searchedOrderItem.Order = searchedOrder;
        //searchedOrderItem.Item = searchedItem;
        if (searchedOrder != null) 
        {
            if (!searchedOrder.Completed && orderItem.Quantity > 0)
            {
                // Update item quantity
                searchedOrderItem.Quantity = orderItem.Quantity;

                _orderRepository.UpdateItemAtOrder(searchedOrderItem);
                return searchedOrderItem;
            } else if (searchedOrder.Completed) {
                throw new Exception("Completed order can not be changed.");
            } else if(orderItem.Quantity <= 0) {
                throw new Exception("Quantity cannot be less than one!");        
            } else {                            
                throw new Exception("Invalid Order! Please check date or order id");
            }
        } else {
            throw new Exception("Invalid Order! Does not exist.");
        }
    }

    public string DeleteItemAtOrder(int orderId, int itemId)
    {
        OrderItem searchedOrderItem = _orderRepository.GetOrderItemById(orderId, itemId);
        Order searchedOrder = _orderRepository.GetOrderById(orderId);

        if (searchedOrder != null) {
             if (!searchedOrder.Completed) {
                //Delete
                _orderRepository.DeleteItemAtOrder(searchedOrderItem);
                return $"Item {_itemRepository.GetItemById(searchedOrderItem.ItemId).Name} is deleted successfully.";
             } else {
                throw new Exception("Item in the completed order cannot be deleted.");
             }
        } else {
            throw new Exception("Invalid order! Does not exist.");
        }
    }
}