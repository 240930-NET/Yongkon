using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
using QuickOrder.API.Data;
using QuickOrder.API.Model;
using QuickOrder.API.Repository;
using QuickOrder.API.Service;
using Xunit.Abstractions;

namespace QuickOrder.TESTS;

public class OrderServiceTest
{
    private readonly ITestOutputHelper _output;

    public OrderServiceTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void GetAllOrdersReturnList()
    {
        //Arrange
        Mock<IOrderRepository> mockOrderRepo = new();
        Mock<IItemRepository> mockItemRepo = new();
        Mock<OrderContext> mockContext = new();
        
        OrderService orderService = new(mockOrderRepo.Object, mockItemRepo.Object, mockContext.Object);
        
        List<Order> orderList = [
            new Order{Id = 1, Date=DateTime.Parse("2024/10/21"), Completed = false},
            new Order{Id = 2, Date=DateTime.Parse("2024/10/22"), Completed = false},
            new Order{Id = 3, Date=DateTime.Parse("2024/10/23"), Completed = false}
        ];

        mockOrderRepo.Setup(repo => repo.GetAllOrders()).Returns(orderList);

        //Act
        var returnedList = orderService.GetAllOrders();

        //Assert
        Assert.NotEmpty(returnedList);
        Assert.Equal(3, returnedList.Count());
        Assert.Contains(returnedList, order => order.Date == DateTime.Parse("2024/10/23"));
    }


    [Fact]
    public void GetAllOrdersThrowExceptionsEmpty()
    {
        //Arrange
        Mock<IOrderRepository> mockOrderRepo = new();
        Mock<IItemRepository> mockItemRepo = new();
        Mock<OrderContext> mockContext = new();
        
        OrderService orderService = new(mockOrderRepo.Object, mockItemRepo.Object, mockContext.Object);
        
        List<Order> orderList = [];

        mockOrderRepo.Setup(repo => repo.GetAllOrders()).Returns(orderList);

        //Act        
        //Assert
        Assert.ThrowsAny<Exception>(() => orderService.GetAllOrders());
    }    

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetOrderByIdReturnsProperOrder(int id)
    {
        //Arrange
        Mock<IOrderRepository> mockOrderRepo = new();
        Mock<IItemRepository> mockItemRepo = new();
        Mock<OrderContext> mockContext = new();
        
        OrderService orderService = new(mockOrderRepo.Object, mockItemRepo.Object, mockContext.Object);
        
        List<Order> orderList = [
            new Order{Id = 1, Date=DateTime.Parse("2024/10/21"), Completed = false},
            new Order{Id = 2, Date=DateTime.Parse("2024/10/22"), Completed = false},
            new Order{Id = 3, Date=DateTime.Parse("2024/10/23"), Completed = false}
        ];

        mockOrderRepo.Setup(repo => repo.GetOrderById(It.IsAny<int>()))
              .Returns(orderList.FirstOrDefault(order => order.Id == id));

        //Act
        var order = orderService.GetOrderById(id);

        //Assert
        Assert.NotNull(order);       
        Assert.IsType<Order>(order);
        Assert.Equal(id, order.Id);
    }


    [Fact]
    public void AddNewOrderToOrderList()
    {
        //Arrange
        Mock<IOrderRepository> mockOrderRepo = new();
        Mock<IItemRepository> mockItemRepo = new();
        Mock<OrderContext> mockContext = new();
        
        OrderService orderService = new(mockOrderRepo.Object, mockItemRepo.Object, mockContext.Object);
        
        List<Order> orderList = [
            new Order{Id = 1, Date=DateTime.Parse("2024/10/21"), Completed = false},
            new Order{Id = 2, Date=DateTime.Parse("2024/10/22"), Completed = false},
            new Order{Id = 3, Date=DateTime.Parse("2024/10/23"), Completed = false}
        ];

        // New Order
        Order newOrder = new(){Id = 4, Date=DateTime.Parse("2024/10/24"), Completed = false};

        // Mock Repo : AddOrder
        mockOrderRepo.Setup(repo => repo.AddOrder(It.IsAny<Order>()))
           .Callback(() => orderList.Add(newOrder));
           
        //Act        
        var result = orderService.AddOrder(newOrder);

        //Assert
        Assert.NotNull(result);
        Assert.Contains(orderList, o => o.Date.Equals(DateTime.Parse("2024/10/24")));
    }      


    [Fact]
    public void EditOrderFromOrderList()
    {
        //Arrange
        Mock<IOrderRepository> mockOrderRepo = new();
        Mock<IItemRepository> mockItemRepo = new();
        Mock<OrderContext> mockContext = new();
        
        OrderService orderService = new(mockOrderRepo.Object, mockItemRepo.Object, mockContext.Object);
        
        List<Order> orderList = [
            new Order{Id = 1, Date=DateTime.Parse("2024/10/21"), Completed = false},
            new Order{Id = 2, Date=DateTime.Parse("2024/10/22"), Completed = false},
            new Order{Id = 3, Date=DateTime.Parse("2024/10/23"), Completed = false}
        ];

        // Edit Order
        Order editOrder = new(){Id = 2, Date=DateTime.Parse("2021/10/24"), Completed = true};

        // Mock Repo : GetOrderById
        mockOrderRepo.Setup(repo => repo.GetOrderById(It.IsAny<int>()))
           .Returns((int id) => {
                var order = orderList.FirstOrDefault(o => o.Id == id);
                return order;
            });

        //Act        
        var result = orderService.UpdateOrder(editOrder);

        //Assert
        Assert.NotNull(result);
        Assert.Contains(orderList, o => o.Date.Date == editOrder.Date.Date);
        Assert.DoesNotContain(orderList, o => o.Date.Date == DateTime.Parse("2024/10/22").Date);        
    }      

    [Fact]
    public void DeleteOrderFromOrderList()
    {
        //Arrange
        Mock<IOrderRepository> mockOrderRepo = new();
        Mock<IItemRepository> mockItemRepo = new();
        Mock<OrderContext> mockContext = new();
        
        OrderService orderService = new(mockOrderRepo.Object, mockItemRepo.Object, mockContext.Object);
        
        List<Order> orderList = [
            new Order{Id = 1, Date=DateTime.Parse("2024/10/21"), Completed = false},
            new Order{Id = 2, Date=DateTime.Parse("2024/10/22"), Completed = false},
            new Order{Id = 3, Date=DateTime.Parse("2024/10/23"), Completed = true}
        ];

        List<OrderItem> orderItemList = [
            new OrderItem{OrderId=1, ItemId=1, Quantity= 1},
            new OrderItem{OrderId=1, ItemId=2, Quantity= 2},
            new OrderItem{OrderId=1, ItemId=3, Quantity= 3},
            new OrderItem{OrderId=2, ItemId=4, Quantity= 4},
            new OrderItem{OrderId=2, ItemId=5, Quantity= 5},
            new OrderItem{OrderId=2, ItemId=6, Quantity= 6},
            new OrderItem{OrderId=3, ItemId=7, Quantity= 7},
            new OrderItem{OrderId=3, ItemId=8, Quantity= 8}
        ];

        // Edit Order
        Order deleteOrder = new(){Id = 2, Date=DateTime.Parse("2024/10/22"), Completed = false};

        // Delete Order
        int id = 2;

        // Mock Repo : GetOrderById
        mockOrderRepo.Setup(repo => repo.GetOrderById(It.IsAny<int>()))
           .Returns((int id) => {
                var order = orderList.FirstOrDefault(o => o.Id == id);
                return order;
            });

        // Mock Repo : Delete Order
        mockOrderRepo.Setup(repo => repo.DeleteOrder(It.IsAny<Order>()))
           .Callback<Order>(order =>
           {
                var searchedOrder = orderList.FirstOrDefault(o => o.Id == order.Id);

                if (searchedOrder != null && !searchedOrder.Completed) {
                    // remove order
                    orderList.Remove(order);

                    // remove item at order
                    orderItemList.RemoveAll(i => i.OrderId == order.Id);                    
                }
           }
           );

        //Act        
        var result = orderService.DeleteOrder(id);

        //Assert
        Assert.DoesNotContain(orderList, o => o.Date.Date == DateTime.Parse("2024/10/22").Date);        
        Assert.DoesNotContain(orderItemList, oi => oi.OrderId == deleteOrder.Id);        
        Assert.DoesNotContain(orderItemList, oi => oi.ItemId == 4 || oi.ItemId == 5 || oi.ItemId == 6);        
    }      

    [Fact]
    public void CannotDeleteCompleteOrder()
    {
        //Arrange
        Mock<IOrderRepository> mockOrderRepo = new();
        Mock<IItemRepository> mockItemRepo = new();
        Mock<OrderContext> mockContext = new();
        
        OrderService orderService = new(mockOrderRepo.Object, mockItemRepo.Object, mockContext.Object);
        
        List<Order> orderList = [
            new Order{Id = 1, Date=DateTime.Parse("2024/10/21"), Completed = false},
            new Order{Id = 2, Date=DateTime.Parse("2024/10/22"), Completed = false},
            new Order{Id = 3, Date=DateTime.Parse("2024/10/23"), Completed = true}
        ];

        List<OrderItem> orderItemList = [
            new OrderItem{OrderId=1, ItemId=1, Quantity= 1},
            new OrderItem{OrderId=1, ItemId=2, Quantity= 2},
            new OrderItem{OrderId=1, ItemId=3, Quantity= 3},
            new OrderItem{OrderId=2, ItemId=4, Quantity= 4},
            new OrderItem{OrderId=2, ItemId=5, Quantity= 5},
            new OrderItem{OrderId=2, ItemId=6, Quantity= 6},
            new OrderItem{OrderId=3, ItemId=7, Quantity= 7},
            new OrderItem{OrderId=3, ItemId=8, Quantity= 8}
        ];

        // Delete Order
        Order deleteOrder = new(){Id = 3, Date=DateTime.Parse("2024/10/23"), Completed = true};

        // Delete Order
        int id = 3;

        // Mock Repo : GetOrderById
        mockOrderRepo.Setup(repo => repo.GetOrderById(It.IsAny<int>()))
           .Returns((int id) => {
                var order = orderList.FirstOrDefault(o => o.Id == id);
                return order;
            });

        // Mock Repo : Delete Order
        mockOrderRepo.Setup(repo => repo.DeleteOrder(It.IsAny<Order>()))
           .Callback<Order>(order =>
           {
                var searchedOrder = orderList.FirstOrDefault(o => o.Id == order.Id);

                if (searchedOrder != null && !searchedOrder.Completed) {
                    // remove order
                    orderList.Remove(order);

                    // remove item at order
                    orderItemList.RemoveAll(i => i.OrderId == order.Id);                    
                }
           }
           );

        //Act        
        //Assert
        Assert.ThrowsAny<Exception>(() => orderService.DeleteOrder(id));
    }   

}