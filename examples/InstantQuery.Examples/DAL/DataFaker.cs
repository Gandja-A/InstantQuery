
using Bogus;

namespace InstantQuery.Examples.DAL
{
    public class DataFaker
    {
        public DataFaker()
        {
            Randomizer.Seed = new Random(3897000);
        }

        public User[] GetUsers()
        {
            var userIds = 1;
            var testUsers = new Faker<User>()
                .RuleFor(u => u.Id, _ => userIds++)
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Age, f => f.Random.Int(0, 100))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.CartId, _ => Guid.NewGuid());
            var data = testUsers.Generate(100).ToArray();
            return data;
        }

        public Order[] GetOrders()
        {
            var fruit = new[] { "Apple", "Banana", "Orange", "Strawberry", "Kiwi" };

            var orderIds = 1;
            var testOrders = new Faker<Order>()
                .RuleFor(o => o.Id, _ => orderIds++)
                .RuleFor(o => o.Item, f => f.PickRandom(fruit))
                .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
                .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now))
                .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 1000))
                .RuleFor(o => o.UserId, f => f.Random.Int(1, 100))
                .RuleFor(o => o.OrderStatusId, f => f.Random.Int(1, 5));
            var data = testOrders.Generate(100).ToArray();
            return data;
        }

        public OrderStatus[] GetOrderStatuses()
        {
            var orderStatuses = new[]
            {
                new OrderStatus { Id = 1, Name = "Pending" }, new OrderStatus { Id = 2, Name = "Confirmed" },
                new OrderStatus { Id = 3, Name = "Paid" }, new OrderStatus { Id = 4, Name = "Canceled" },
                new OrderStatus { Id = 5, Name = "Shipped" }
            };

            return orderStatuses;
        }
    }
}
