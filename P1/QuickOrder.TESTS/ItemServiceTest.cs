using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
using QuickOrder.API.Model;
using QuickOrder.API.Repository;
using QuickOrder.API.Service;


namespace QuickOrder.TESTS;

public class ItemServiceTest
{
    [Fact]
    public void GetAllItemsReturnList()
    {
        //Arrange
        Mock<IItemRepository> mockRepo = new();
        ItemService itemService = new(mockRepo.Object);
        
        List<Item> itemList = [
            new Item{Id = 1, Name = "Water", Price = 2.5},
            new Item{Id = 2, Name = "Snack", Price = 1.5},
            new Item{Id = 3, Name = "Apple", Price = 1.2}
        ];

        mockRepo.Setup(repo => repo.GetAllItems()).Returns(itemList);

        //Act
        var returnedList = itemService.GetAllItems();

        //Assert
        Assert.NotEmpty(returnedList);
        Assert.Equal(3, returnedList.Count());
        Assert.Contains(returnedList, item => item.Name.Equals("Apple"));
    }


    [Fact]
    public void GetAllItemsThrowExceptionsEmpty()
    {
        //Arrange
        Mock<IItemRepository> mockRepo = new();
        ItemService itemService = new(mockRepo.Object);
        
        List<Item> itemList = [];

        mockRepo.Setup(repo => repo.GetAllItems()).Returns(itemList);

        //Act        
        //Assert
        Assert.ThrowsAny<Exception>(() => itemService.GetAllItems());
    }    

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetItemsByIdReturnsProperUser(int id)
    {
        //Arrange
        Mock<IItemRepository> mockRepo = new();
        ItemService itemService = new(mockRepo.Object);
        
        List<Item> itemList = [
            new Item{Id = 1, Name = "Water", Price = 2.5},
            new Item{Id = 2, Name = "Snack", Price = 1.5},
            new Item{Id = 3, Name = "Apple", Price = 1.2}
        ];

        mockRepo.Setup(repo => repo.GetItemById(It.IsAny<int>()))
              .Returns(itemList.FirstOrDefault(item => item.Id == id));

        //Act
        var item = itemService.GetItemById(id);

        //Assert
        Assert.NotNull(item);       
        Assert.IsType<Item>(item);
        Assert.Equal(id, item.Id);
    }


    [Fact]
    public void AddNewItemToItemList()
    {
        //Arrange
        Mock<IItemRepository> mockRepo = new();
        ItemService itemService = new(mockRepo.Object);
        
        List<Item> itemList = [
            new Item{Id = 1, Name = "Water", Price = 2.5},
            new Item{Id = 2, Name = "Snack", Price = 1.5},
            new Item{Id = 3, Name = "Apple", Price = 1.2}
        ];

 
        Item newItem = new(){Id = 4, Name = "Chicken", Price = 4.99};

        mockRepo.Setup(repo => repo.AddItem(It.IsAny<Item>()))
           .Callback(() => itemList.Add(newItem));
           
        //Act        
        var result = itemService.AddItem(newItem);

        //Assert
        Assert.NotNull(result);
        Assert.Contains(itemList, i => i.Name.Equals("Chicken"));
    }        


    [Fact]
    public void EditItemFromItemList()
    {
        //Arrange
        Mock<IItemRepository> mockRepo = new();
        ItemService itemService = new(mockRepo.Object);
        
        List<Item> itemList = [
            new Item{Id = 1, Name = "Water", Price = 2.49},
            new Item{Id = 2, Name = "Snack", Price = 1.49},
            new Item{Id = 3, Name = "Apple", Price = 0.99}
        ];

        // Update Item
        Item updateItem = new(){Id = 2, Name = "Chips", Price = 1.99};

        // Moc Repo : GetItemById
        mockRepo.Setup(repo => repo.GetItemById(It.IsAny<int>()))
           .Returns((int id) => itemList.FirstOrDefault(i => i.Id == id));

        //Act        
        var result = itemService.UpdateItem(updateItem);

        //Assert
        Assert.NotNull(result);
        Assert.Contains(itemList, i => i.Name.Equals("Chips"));
    }  

    [Fact]
    public void RemoveItemFromItemList()
    {
        //Arrange
        Mock<IItemRepository> mockRepo = new();
        ItemService itemService = new(mockRepo.Object);
        
        List<Item> itemList = [
            new Item{Id = 1, Name = "Water", Price = 2.49},
            new Item{Id = 2, Name = "Snack", Price = 1.49},
            new Item{Id = 3, Name = "Apple", Price = 0.99}
        ];

        // Delete Item Id
        int id = 2;

        // Moc Repo : GetItemById
        mockRepo.Setup(repo => repo.GetItemById(It.IsAny<int>()))
           .Returns((int id) => itemList.FirstOrDefault(i => i.Id == id));

        // Moc Repo : DeleteItem
        
        mockRepo.Setup(repo => repo.DeleteItem(It.IsAny<Item>()))
            .Callback<Item>(item => 
            {
                var selectedItem = itemList.Find(i => i.Id == item.Id);
                if (selectedItem != null) {
                    itemList.Remove(selectedItem);
                }
            });

        //Act        
        var result = itemService.DeleteItem(id);

        //Assert
        Assert.NotNull(result);
        Assert.DoesNotContain(itemList, i => i.Name.Equals("Snack"));
    }  
}