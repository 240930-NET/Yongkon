using QuickOrder.API.Model;
using QuickOrder.API.Repository;

namespace QuickOrder.API.Service;

public class ItemService : IItemService 
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository) => _itemRepository = itemRepository;

    public IEnumerable<Item> GetAllItems() 
    {
        IEnumerable<Item> items = _itemRepository.GetAllItems();
        if (items.Count() == 0) {
            throw new Exception("No Items were found");
        } else {
            return items;
        }        
    }

    public Item GetItemById(int id) 
    {
        Item item = _itemRepository.GetItemById(id);
        
        if (item == null) {
            throw new Exception($"{id} Item not found");
        } else {
            return item;
        }
    }

    public string AddItem(Item item) 
    {
        if (item.Name != null && item.Price > 0) 
        {
            _itemRepository.AddItem(item);
            return $"Item {item.Name} added successfully!";
        } else {
            throw new Exception("Invalid item. Please check the item information.");
        }
    }

    public Item UpdateItem(Item item)
    {
        Item searchedItem = _itemRepository.GetItemById(item.Id);
        if (searchedItem != null) 
        {
            if (searchedItem.Name != null && searchedItem.Price >0)
            {
                //Update Item
                searchedItem.Name = item.Name;
                searchedItem.Price = item.Price;

                _itemRepository.UpdateItem(searchedItem);
                return searchedItem;
            } else {
                throw new Exception("Invalid item! Please check name or price");
            }
        } else {
            throw new Exception("Invalid Item! Does not exist.");
        }
    }

    public string DeleteItem(int id)
    {
        Item searchedItem = _itemRepository.GetItemById(id);

        if (searchedItem != null) {
            //Delete
            _itemRepository.DeleteItem(searchedItem);
            return $"Item {searchedItem.Name} is deleted successfully.";
        } else {
            throw new Exception("Invalid Item! Does not exist.");
        }
    }
}