public static class OrderBookClient{
    public static void Run(){
        IOrderBook book = new OrderBook();
        Order o1 = new Order(OrderType.Sell, 500, 30);
        Order o2 = new Order(OrderType.Sell, 100, 35);
        book.PlaceOrder(o1);
        book.PlaceOrder(o2);

        //book.CancelOrder(o1.Id);

        Order o6 = new Order(OrderType.Buy, 100, 30);
        Order o7 = new Order(OrderType.Buy, 100, 30);

        book.PlaceOrder(o6);
        book.PlaceOrder(o7);

        Console.WriteLine(book.GetAmout(30, OrderType.Buy));
        Console.WriteLine(book.GetAmout(30, OrderType.Sell));
        
        Order o8 = new Order(OrderType.Sell, 75, 30);
        book.PlaceOrder(o8);

        Console.WriteLine(book.GetAmout(30, OrderType.Buy));
        Console.WriteLine(book.GetAmout(30, OrderType.Sell));

        Order o9 = new Order(OrderType.Sell, 25, 30);
        book.PlaceOrder(o9);

        Console.WriteLine(book.GetAmout(30, OrderType.Buy));
        Console.WriteLine(book.GetAmout(30, OrderType.Sell));
    }
}