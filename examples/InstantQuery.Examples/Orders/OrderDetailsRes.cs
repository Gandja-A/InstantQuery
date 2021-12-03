
namespace InstantQuery.Examples.Orders
{
    public class OrderDetailsRes
    {
        public long OrderId { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public int? LotNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserFullName { get; set; }
        public long UserId { get; set; }
        public string StatusName { get; set; }
    }
}
