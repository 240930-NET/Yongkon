using QuickOrder.API.Data;
using QuickOrder.API.Model;

namespace QuickOrder.API.Repository;

public class ItemRepository : IItemRepository 
{
    private readonly OrderContext _itemContext;

    public ItemRepository(OrderContext itemContext) => _itemContext = itemContext;

    public IEnumerable<Item> GetAllItems()
    {
        return _itemContext.Items.ToList();
    }

    public Item GetItemById(int id)
    {
         var item = _itemContext.Items.FirstOrDefault(i => i.Id == id);

         if (item == null) {
            throw new Exception("Item not found");
         } else {
            return item;
         }
    }

   public void AddItem(Item item)
    {
        _itemContext.Items.Add(item);
        _itemContext.SaveChanges();
        
    }

    public void UpdateItem(Item item)
    {
        _itemContext.Items.Update(item);
        _itemContext.SaveChanges();
    }
    
    public void DeleteItem(Item item)
    {
        _itemContext.Items.Remove(item);
        _itemContext.SaveChanges();
    }

}