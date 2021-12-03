using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.Filters;
using Xunit;

namespace InstantQuery.UnitTests
{
    public class QueryConfigurationTests
    {
        [Fact]
        public void QueryConfiguration_FilterModelWithoutAttribute_ReturnTheSameList()
        {
            var filter = new { DateTimeOffsetNullableValue = "11/10/2021 2:27:45 PM" };

            var dataList = new List<Entity>
            {
                new() { DateTimeOffsetNullableValue = null },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/10/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().And.BeEquivalentTo(dataList);
        }

        [Fact]
        public void QueryConfiguration_FilterParameterIsNullOneFitDateTimeOffsetNullable_ReturnTheSameList()
        {
            var filter = new FilterQueryConfiguration { DateTimeOffsetNullableValue = null };

            var dataList = new List<Entity>
            {
                new() { DateTimeOffsetNullableValue = null },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/10/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().And.BeEquivalentTo(dataList);
        }

        [Fact]
        public void QueryConfiguration_TwoAttributesApplyToField_ShouldThrowAnException()
        {
            var filter = new FilterQueryConfTwoAttributeApplyToField { DateTimeOffsetNullableValue = null };
            var dataList = new List<Entity>
            {
                new() { DateTimeOffsetNullableValue = null },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/10/2021 2:27:45 PM") }
            }.AsQueryable();

            Assert.Throws<ArgumentException>(() => dataList.Filter(filter).ToList());
        }
    }
}
