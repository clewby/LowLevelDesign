public interface IOrderBook{
    public Guid PlaceOrder(Order order);
    public void CancelOrder(Guid orderId);
    public int GetAmout(int price, OrderType type);
}