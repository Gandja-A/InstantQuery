
using Microsoft.EntityFrameworkCore;

namespace InstantQuery.Examples.DAL
{
    public class ExamplesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public ExamplesDbContext(DbContextOptions options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.SeedData(modelBuilder);

        }

        protected void SeedData(ModelBuilder modelBuilder)
        {
            var dataFaker = new DataFaker();

            modelBuilder.Entity<User>().HasData(dataFaker.GetUsers());

            modelBuilder.Entity<Order>().HasData(dataFaker.GetOrders());

            modelBuilder.Entity<OrderStatus>().HasData(dataFaker.GetOrderStatuses());
        }
    }
}
