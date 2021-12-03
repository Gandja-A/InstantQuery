using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.Filters;
using Xunit;

namespace InstantQuery.UnitTests
{
    public class SearchFilterTests
    {
        [Fact]
        public void SearchStartWithStrategy_TwoFitStringValues_ReturnListOfTwoElements()
        {
            var filter = new FilterSearchStartsWith { StringValue = "a" };

            var dataList = new List<Entity>
            {
                new() { StringValue = "aca" },
                new() { StringValue = "A" },
                new() { StringValue = "b" },
                new() { StringValue = "c" }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should()
                .BeEquivalentTo(new[] { new Entity { StringValue = "aca" }, new Entity { StringValue = "A" } });
        }
    }
}
