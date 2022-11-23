public class OrderBook : IOrderBook
{
    private Dictionary<(int, OrderType), int> priceAmountMap = new();
    private Dictionary<Guid, OrderType> orderTypeMap = new();
    private Dictionary<Guid, LinkedListNode<Order>> listNodesMap = new();
    
    private SortedDictionary<int, LinkedList<Order>> sellHeap = new(); //min heap
    private SortedDictionary<int, LinkedList<Order>> buyHeap = new(new MaxComparer()); //max heap

    private class MaxComparer : IComparer<int>{
        public int Compare(int x, int y) { return y-x; }
    }

    public OrderBook(){}

    // O(1) - time
    public void CancelOrder(Guid orderId){
        OrderType type = orderTypeMap[orderId];
        SortedDictionary<int, LinkedList<Order>> heap = orderTypeMap[orderId] == OrderType.Buy ? buyHeap : sellHeap;
        LinkedListNode<Order> orderNode = listNodesMap[orderId];
        LinkedList<Order> list = heap[orderNode.Value.Price];
        orderTypeMap.Remove(orderId);
        list.Remove(orderNode);
        priceAmountMap[(orderNode.Value.Price, type)] -= orderNode.Value.Amount;
        if(priceAmountMap[(orderNode.Value.Price, type)] <= 0) priceAmountMap.Remove((orderNode.Value.Price, type));
        if(list.Count <= 0) heap.Remove(orderNode.Value.Price);
        Console.WriteLine("Cancelled "+orderId);
    }

    // O(1) time operation
    public int GetAmout(int price, OrderType type){
        if(!priceAmountMap.ContainsKey((price, type)))
            return 0;
        return priceAmountMap[(price, type)];
    }

    // N - number of heap elements
    // K - number of orders with the same or better price
    // O(N*K) - time?
    public Guid PlaceOrder(Order order){
        if(order.Amount <= 0) throw new Exception("Order Amount must be more than 0.");
        
        switch (order.Type)
        {
            case OrderType.Buy:
                return PlaceBuyOrder(order);
            case OrderType.Sell:
                return PlaceSellOrder(order);
            default:
                throw new Exception("Order Amount must be more than 0.");   
        }
    }

    private Guid PlaceBuyOrder(Order buyOrder){
        while(sellHeap.Count > 0 && buyOrder.Amount > 0 && sellHeap.First().Key <= buyOrder.Price){
            
            LinkedList<Order> sellList = sellHeap.First().Value;

            while(buyOrder.Amount > 0 && sellList.Count > 0){
                Order sellOrder = sellList.First();
                int sellOrderAmount = Math.Min(sellOrder.Amount, buyOrder.Amount);
                sellOrder.UpdateAmount(sellOrderAmount*-1);
                buyOrder.UpdateAmount(sellOrderAmount*-1);
                priceAmountMap[(sellOrder.Price, OrderType.Sell)] -= sellOrderAmount;

                Console.WriteLine($"Sell Order {sellOrder.Id} has been executed. Amount of shares sold {sellOrderAmount} by price {sellOrder.Price}.");
                Console.WriteLine($"Buy Order {buyOrder.Id} has been executed. Amount of shares bought {sellOrderAmount} by price {sellOrder.Price}.");
                
                if(sellOrder.Amount <= 0) CancelOrder(sellOrder.Id);
            }
        }
        //O(1) - time
        if(buyOrder.Amount > 0){
            
            if(!buyHeap.ContainsKey(buyOrder.Price))
                buyHeap.Add(buyOrder.Price, new LinkedList<Order>());

            LinkedListNode<Order> buyOrderNode = buyHeap[buyOrder.Price].AddLast(buyOrder);
            listNodesMap.Add(buyOrder.Id, buyOrderNode);
            orderTypeMap.Add(buyOrder.Id, OrderType.Buy);

            if(!priceAmountMap.ContainsKey((buyOrder.Price, OrderType.Buy)))
                priceAmountMap.Add((buyOrder.Price, OrderType.Buy), 0);
            priceAmountMap[(buyOrder.Price, OrderType.Buy)] += buyOrder.Amount;

            Console.WriteLine($"Buy Order {buyOrder.Id} has been added. Amount of shares to buy {buyOrder.Amount} by price {buyOrder.Price}.");
        }
        return buyOrder.Id;
    }

    private Guid PlaceSellOrder(Order sellOrder){
        while(buyHeap.Count > 0 && sellOrder.Amount > 0 && buyHeap.First().Key >= sellOrder.Price){
            
            LinkedList<Order> buyList = buyHeap.First().Value;
            
            while(sellOrder.Amount > 0 && buyList.Count > 0){
                Order buyOrder = buyList.First();
                int buyOrderAmount = Math.Min(sellOrder.Amount, buyOrder.Amount);
                sellOrder.UpdateAmount(buyOrderAmount*-1);
                buyOrder.UpdateAmount(buyOrderAmount*-1);
                priceAmountMap[(buyOrder.Price, OrderType.Buy)] -= buyOrderAmount;

                Console.WriteLine($"Sell Order {sellOrder.Id} has been executed. Amount of shares sold {buyOrderAmount} by price {sellOrder.Price}.");
                Console.WriteLine($"Buy Order {buyOrder.Id} has been executed. Amount of shares bought {buyOrderAmount} by price {sellOrder.Price}.");
                
                if(buyOrder.Amount <= 0) CancelOrder(buyOrder.Id);
            }
        }
        //O(1) - time
        if(sellOrder.Amount > 0){

            if(!sellHeap.ContainsKey(sellOrder.Price))
                sellHeap.Add(sellOrder.Price, new LinkedList<Order>());

            LinkedListNode<Order> sellOrderNode = sellHeap[sellOrder.Price].AddLast(sellOrder);
            listNodesMap.Add(sellOrder.Id, sellOrderNode);
            orderTypeMap.Add(sellOrder.Id, OrderType.Sell);

            if(!priceAmountMap.ContainsKey((sellOrder.Price, OrderType.Sell)))
                priceAmountMap.Add((sellOrder.Price, OrderType.Sell), 0);
            priceAmountMap[(sellOrder.Price, OrderType.Sell)] += sellOrder.Amount;

            Console.WriteLine($"Sell Order {sellOrder.Id} has been added. Amount of shares to sell {sellOrder.Amount} by price {sellOrder.Price}.");
        }
        return sellOrder.Id;
    }
}