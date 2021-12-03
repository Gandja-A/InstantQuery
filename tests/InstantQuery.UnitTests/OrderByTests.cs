using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using Xunit;

namespace InstantQuery.UnitTests
{
    public class OrderByTests
    {
        [Fact]
        public void OrderBy_SortBySingleField_ReturnOrderedListByAscending()
        {
            var dataList = new List<Entity>
            {
                new() { IntValue = 3 }, new() { IntValue = 1 }, new() { IntValue = 4 }, new() { IntValue = 2 }
            }.AsQueryable();

            var result = dataList.OrderBy("IntValue", "asc").ToList();

            result.Should().BeInAscendingOrder(a => a.IntValue);
        }

        [Fact]
        public void OrderBy_SortByTwoFields_ReturnOrderedListByTowFieldsByAscending()
        {
            var dataList = new List<Entity>
            {
                new() { IntValue = 1, StringValue = "Aaaa" },
                new() { IntValue = 1, StringValue = "Mmmm" },
                new() { IntValue = 1, StringValue = "Cccc" },
            }.AsQueryable();

            var result = dataList.OrderBy("IntValue, StringValue", "asc").ToList();

            result.Should().BeInAscendingOrder(s => s.IntValue);
            result.Should().BeInAscendingOrder(s => s.StringValue);
        }

        [Fact]
        public void OrderBy_SortByTwoFieldsWithDifferentDirections_ReturnListOrderedByAscendingByFirsFiledAndThanOrderedByDescendingBySecondField()
        {
            var dataList = new List<Entity>
            {
                new() { IntValue = 1, StringValue = "Aaaa" },
                new() { IntValue = 1, StringValue = "Mmmm" },
                new() { IntValue = 1, StringValue = "Cccc" },
            }.AsQueryable();

            var result = dataList.OrderBy("IntValue, StringValue", "asc, desc").ToList();

            result.Should().BeInAscendingOrder(s => s.IntValue);
            result.Should().BeInDescendingOrder(s => s.StringValue);
        }

        [Fact]
        public void OrderBy_SortByTwoFieldsWithEmptyDirections_ReturnOrderedListByFieldsByAscending()
        {
            var dataList = new List<Entity>
            {
                new() { IntValue = 1, StringValue = "Aaaa" },
                new() { IntValue = 1, StringValue = "Mmmm" },
                new() { IntValue = 1, StringValue = "Cccc" },
            }.AsQueryable();

            var result = dataList.OrderBy("IntValue, StringValue", string.Empty).ToList();

            result.Should().BeInAscendingOrder(s => s.IntValue);
            result.Should().BeInAscendingOrder(s => s.StringValue);
        }
    }
}
