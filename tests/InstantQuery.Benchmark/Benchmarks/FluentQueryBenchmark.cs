using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using InstantQuery.Benchmark.Data;
using Microsoft.EntityFrameworkCore;

namespace InstantQuery.Benchmark.Benchmarks
{
    public class FluentQueryBenchmark
    {
        private BenchmarkDbContext dbContext;

        private Filter filter;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            this.dbContext = BenchmarkDbContext.CreateContext();
            this.filter = new Filter
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                SearchTerm = "a",
                SortBy = "lotNumber",
                SortDir = "asc",
                StatusIds = new List<long>() { 1, 2, 3, 4 },
                Page = 1,
                PageSize = 20,
            };
            Console.WriteLine(await this.dbContext.Orders.CountAsync());

        }

        [Benchmark(Baseline = true)]
        public async Task VanillaLinqQuery()
        {
            var query = this.dbContext.Orders.Select(o => new OrderDetailsDto
            {
                UserFullName = o.User.FirstName + " " + o.User.LastName,
                CreatedAt = o.CreatedAt,
                StatusName = o.OrderStatus.Name,
                OrderStatusId = o.OrderStatusId,
                Item = o.Item,
                LotNumber = o.LotNumber,
                OrderId = o.Id,
                Quantity = o.Quantity,
                UserId = o.UserId
            });

            if(filter.StartDate != null && filter.EndDate != null)
            {
                query = query.Where(o => o.CreatedAt.Date >= filter.StartDate && o.CreatedAt.Date <= filter.EndDate);
            }

            if(filter.StartDate != null && filter.EndDate == null)
            {
                query = query.Where(o => o.CreatedAt.Date >= filter.StartDate);
            }

            if(filter.StartDate != null && filter.EndDate == null)
            {
                query = query.Where(o => o.CreatedAt.Date <= filter.EndDate);
            }

            if(!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(q => q.UserFullName.ToLower().Contains(filter.SearchTerm.ToLower()));
            }

            if(filter.StatusIds.Any())
            {
                query = query.Where(q => filter.StatusIds.Contains(q.OrderStatusId));
            }

            if(!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sortDir = !string.IsNullOrWhiteSpace(filter.SortDir) ? filter.SortDir : "asc";

                if(sortDir == "asc")
                {
                    query = query.OrderBy(f => f.LotNumber);
                }
                else
                {
                    query = query.OrderByDescending(f => f.LotNumber);
                }
            }

            var count = await query.CountAsync();
            var list = await query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        }

        [Benchmark]
        public async Task FluentQuery()
        {
            var query = this.dbContext.Orders.Select(o => new OrderDetailsDto
            {
                UserFullName = o.User.FirstName + " " + o.User.LastName,
                CreatedAt = o.CreatedAt,
                StatusName = o.OrderStatus.Name,
                OrderStatusId = o.OrderStatusId,
                Item = o.Item,
                LotNumber = o.LotNumber,
                OrderId = o.Id,
                Quantity = o.Quantity,
                UserId = o.UserId
            }).FilterAndSort(this.filter);
            var count = await query.CountAsync();
            var list = await query.TakePage(this.filter).ToListAsync();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            this.dbContext.Dispose();
        }
    }
}
