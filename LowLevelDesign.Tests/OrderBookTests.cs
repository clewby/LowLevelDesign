namespace LowLevelDesign.Tests;

public class OrderBookTests
{
    private readonly IOrderBook _orderBook;

    public OrderBookTests()
    {
        _orderBook = new OrderBook();
    }

    [Fact]
    public void Place_Buy_Order()
    {
        Order order = new Order(OrderType.Buy, 100, 100);
        _orderBook.PlaceOrder(order);
        
        Assert.Equal(100, _orderBook.GetAmout(100, OrderType.Buy));
    }
    [Fact]
    public void Place_Two_Buy_Orders()
    {
        Order order1 = new Order(OrderType.Buy, 100, 100);
        Order order2 = new Order(OrderType.Buy, 50, 100);
        _orderBook.PlaceOrder(order1);
        _orderBook.PlaceOrder(order2);
        
        Assert.Equal(150, _orderBook.GetAmout(100, OrderType.Buy));
    }

    [Fact]
    public void Place_Sell_Order()
    {
        Order order = new Order(OrderType.Sell, 100, 100);
        _orderBook.PlaceOrder(order);
        
        Assert.Equal(100, _orderBook.GetAmout(100, OrderType.Sell));
    }

    [Fact]
    public void Place_Buy_Order_And_Cancel()
    {
        Order order = new Order(OrderType.Buy, 100, 100);
        Guid orderId = _orderBook.PlaceOrder(order);
        _orderBook.CancelOrder(orderId);
        Assert.Equal(0, _orderBook.GetAmout(100, OrderType.Buy));
    }

    [Fact]
    public void Place_Sell_Order_And_Cancel()
    {
        Order order = new Order(OrderType.Sell, 100, 100);
        Guid orderId = _orderBook.PlaceOrder(order);
        _orderBook.CancelOrder(orderId);
        Assert.Equal(0, _orderBook.GetAmout(100, OrderType.Sell));
    }

    [Fact]
    public void Place_Two_Buy_Orders_And_Cancel_One()
    {
        Order order1 = new Order(OrderType.Buy, 100, 100);
        Order order2 = new Order(OrderType.Buy, 50, 100);
        Guid orderId1 = _orderBook.PlaceOrder(order1);
        Guid orderId2 = _orderBook.PlaceOrder(order2);
        _orderBook.CancelOrder(orderId1);
        Assert.Equal(50, _orderBook.GetAmout(100, OrderType.Buy));
    }

    [Fact]
    public void Place_Two_Buy_Orders_And_One_Sell_Order()
    {
        Order order1 = new Order(OrderType.Buy, 100, 100);
        Order order2 = new Order(OrderType.Buy, 50, 100);
        Order order3 = new Order(OrderType.Sell, 25, 100);
        Guid orderId1 = _orderBook.PlaceOrder(order1);
        Guid orderId2 = _orderBook.PlaceOrder(order2);
        Guid orderId3 = _orderBook.PlaceOrder(order3);
        Assert.Equal(125, _orderBook.GetAmout(100, OrderType.Buy));
        Assert.Equal(0, _orderBook.GetAmout(100, OrderType.Sell));
    }
}