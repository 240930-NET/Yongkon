using QuickOrder.API.Model;

namespace QuickOrder.API.Repository;

public interface IItemRepository
{
    public IEnumerable<Item> GetAllItems();

    public Item GetItemById(int id);

    public void AddItem(Item item);

    public void UpdateItem(Item item);
    
    public void DeleteItem(Item item);

}

public interface IOrderRepository
{
    public IEnumerable<Order> GetAllOrders();

    public IEnumerable<OrderItem> GetAnOrderWithItems(int orderId);

    public Order GetOrderById(int id);

    public OrderItem GetOrderItemById(int orderId, int itemId);

    //Order
    public void AddOrder(Order order);

    public void UpdateOrder(Order order);
    
    public void DeleteOrder(Order order);
    
    //Item to Order
    public void AddItemToOrder(OrderItem orderItem);
    public void UpdateItemAtOrder(OrderItem orderItem);
    public void DeleteItemAtOrder(OrderItem orderItem);
}