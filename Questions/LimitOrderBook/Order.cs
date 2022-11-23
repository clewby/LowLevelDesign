public class Order{
    public Guid Id { get; private set; }
    public OrderType Type { get; private set; }
    public int Amount { get; private set; }
    public int Price { get; private set; }
    public bool Cancelled { get; set; }
    public DateTime Created { get; private set; }

    public Order(OrderType type, int amount, int price)
    {
        this.Id = Guid.NewGuid();
        this.Created = DateTime.UtcNow;
        this.Type = type;
        this.Amount = amount;
        this.Price = price;
    }
    public void UpdateAmount(int delta){
        this.Amount += delta;
    }
}