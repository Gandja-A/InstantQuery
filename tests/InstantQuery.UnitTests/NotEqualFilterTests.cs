using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.Filters;
using Xunit;

namespace InstantQuery.UnitTests
{
    public class NotEqualFilterTests
    {
        [Fact]
        public void FilterNotEqual_OneFitIntValue_ReturnListOfTwoElements()
        {
            var filter = new FilterNotEqualInt { IntValue = 2 };

            var dataList = new List<Entity>
            {
                new() { IntValue = 0 }, new() { IntValue = 1 }, new() { IntValue = 2 }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterNotEqual_TwoFitStringValues_ReturnListOfTwoElements()
        {
            var filter = new FilterNotEqualString { StringValue = "b" };

            var dataList = new List<Entity>
            {
                new() { IntValue = 0, StringValue = "A", },
                new() { IntValue = 1, StringValue = "A", },
                new() { IntValue = 2, StringValue = "b", },
                new() { IntValue = 5, StringValue = "b", }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterNotEqual_TwoFitIntNullableValues_ReturnListOfTwoElements()
        {
            var filter = new FilterNotEqualNullableLong { LongNullableValue = 3 };

            var dataList = new List<Entity>
            {
                new() { LongNullableValue = null },
                new() { LongNullableValue = 1 },
                new() { LongNullableValue = 3 },
                new() { LongNullableValue = 3 }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterNotEqual_TwoFitDateTimeValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM");
            var filter = new FilterNotEqualDateTime { DateTimeValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeValue = DateTime.Parse("11/9/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM") },
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterNotEqual_TwoFitDateTimeNullableValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM");
            var filter = new FilterNotEqualNullableDateTime { DateTimeNullableValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeNullable = null },
                new() { DateTimeNullable = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterNotEqualFilterParameterIsNullOneFitDateTimeNullable_ReturnTheSameList()
        {
            var filter = new FilterNotEqualNullableDateTime { DateTimeNullableValue = null };

            var dataList = new List<Entity>
            {
                new() { DateTimeNullable = null },
                new() { DateTimeNullable = DateTime.Parse("11/10/2021 2:27:45 PM") },
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().And.BeEquivalentTo(dataList);
        }

        [Fact]
        public void FilterNotEqual_TwoFitDateTimeOffsetNullableValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTimeOffset.Parse("11/11/2021 2:27:45 PM");
            var filter = new FilterNotEqualNullableDateTimeOffset { DateTimeOffsetNullableValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeOffsetNullableValue = null },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }
    }
}
