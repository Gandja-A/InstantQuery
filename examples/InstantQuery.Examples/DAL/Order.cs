namespace InstantQuery.Examples.DAL
{
    public class Order
    {
        public long Id { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public int? LotNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public long OrderStatusId { get; set; }
    }
}
