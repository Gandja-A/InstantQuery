namespace InstantQuery.Examples.DAL
{
    public class OrderStatus
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }
}
