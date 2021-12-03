using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.Filters;
using InstantQuery.UnitTests.MockData;
using Xunit;

namespace InstantQuery.UnitTests
{
    public class SortTests : IDisposable
    {
        private readonly TestDbContext dbContext;

        public SortTests()
        {
            this.dbContext = TestDbContext.CreateContext();
            this.dbContext.AddFiveEntities();
        }

        [Fact]
        public void SortAscending_SortBySingleField_ReturnOrderedListByAscending()
        {
            var sortableFilter = new SortableFilter { SortBy = "IntValue", SortDir = "asc" };

            var dataList = new List<Entity>
            {
                new() { IntValue = 3 }, new() { IntValue = 1 }, new() { IntValue = 4 }, new() { IntValue = 2 }
            }.AsQueryable();

            var result = dataList.Sort(sortableFilter).ToList();

            result.Should().BeInAscendingOrder(a => a.IntValue);
        }

        [Fact]
        public void SortDescending_SortBySingleField_ReturnOrderedListByDescending()
        {
            var sortableFilter = new SortableFilter { SortBy = "IntValue", SortDir = "desc" };

            var dataList = new List<Entity>
            {
                new() { IntValue = 3 }, new() { IntValue = 1 }, new() { IntValue = 4 }, new() { IntValue = 2 }
            }.AsQueryable();

            var result = dataList.Sort(sortableFilter).ToList();

            result.Should().BeInDescendingOrder(a => a.IntValue);
        }

        [Fact]
        public void SortDefaultConventions_SortByFilterIsEmptyInMemoryCollection_ReturnTheSameList()
        {
            var sortableFilter = new SortableFilter { SortBy = "", SortDir = "desc" };

            var dataList = new List<Entity>
            {
                new() { Id = 2, IntValue = 3 },
                new() { Id = 0, IntValue = 1 },
                new() { Id = 3, IntValue = 4 },
                new() { Id = 4, IntValue = 2 }
            }.AsQueryable();

            var result = dataList.Sort(sortableFilter).ToList();

            result.Should().BeEquivalentTo(dataList);
        }

        [Fact]
        public void SortDefaultConventions_SortByFilterIsEmptyDataBaseEntities_ReturnOrderedListByFirstFieldByAscending()
        {
            var sortableFilter = new SortableFilter { SortBy = "", SortDir = "desc" };

            var result = this.dbContext.Entities.Sort(sortableFilter).ToList();

            result.Should().BeInAscendingOrder(a => a.Id);
        }

        public void Dispose()
        {
            this.dbContext?.Dispose();
        }
    }
}
