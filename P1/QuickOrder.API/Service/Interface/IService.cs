using QuickOrder.API.Model;

namespace QuickOrder.API.Service;

public interface IItemService 
{
    public IEnumerable<Item> GetAllItems();

    public Item GetItemById(int id);


    public string AddItem(Item item);

    public Item UpdateItem(Item item);

    public string DeleteItem(int id);
}

public interface IOrderService
{
    public IEnumerable<Order> GetAllOrders();
    public IEnumerable<OrderItem> GetAnOrderWithItems(int orderId);

    public Order GetOrderById(int id);
    public OrderItem GetOrderItemById(int orderId, int itemId);

    // Order
    public string AddOrder(Order order);
    public Order UpdateOrder(Order order);
    public string DeleteOrder(int id);    


    // OrderItem
    public string AddItemToOrder(OrderItem orderItem);
    public OrderItem UpdateItemAtOrder(OrderItem orderItem);
    public string DeleteItemAtOrder(int orderId, int itemId);    
}