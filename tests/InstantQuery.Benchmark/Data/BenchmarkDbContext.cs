
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InstantQuery.Benchmark.Data
{
    public class BenchmarkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public BenchmarkDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.SeedData(modelBuilder);

        }
        public static BenchmarkDbContext CreateContext()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<BenchmarkDbContext>();
            builder.UseSqlite(connection);
            var options = builder.Options;
            var context = new BenchmarkDbContext(options);
            context.Database.EnsureCreated();
            return context;
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
