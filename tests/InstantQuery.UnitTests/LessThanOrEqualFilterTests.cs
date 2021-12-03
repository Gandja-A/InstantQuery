using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.Filters;
using Xunit;

namespace InstantQuery.UnitTests
{
    public class LessThanOrEqualFilterTests
    {
        [Fact]
        public void FilterLessThanOrEqual_TwoFitIntValues_ReturnListOfTwoElements()
        {
            var filter = new FilterLessThanOrEqualInt { IntValue = 2 };

            var dataList = new List<Entity>
            {
                new() { IntValue = 4 }, new() { IntValue = 0 }, new() { IntValue = 1 }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterLessThanOrEqual_TwoFitStringValues_ReturnListOfTwoElements()
        {
            var filter = new FilterLessThanOrEqualString { StringValue = "b" };

            var dataList = new List<Entity>
            {
                new() { StringValue = "a" },
                new() { StringValue = "b" },
                new() { StringValue = "c" },
                new() { StringValue = "c" }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterLessThanOrEqual_TwoFitIntNullableValue_ReturnListOfTwoElements()
        {
            var filter = new FilterLessThanOrEqualNullableLong { LongNullableValue = 3 };

            var dataList = new List<Entity>
            {
                new() { LongNullableValue = null },
                new() { LongNullableValue = 1 },
                new() { LongNullableValue = 3 },
                new() { LongNullableValue = 4 },
                new() { LongNullableValue = 4 },
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterLessThanOrEqual_TwoFitDateTimeValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTime.Parse("11/11/2021 12:00:00 AM");
            var filter = new FilterLessThanOrEqualDateTime { DateTimeValue = dateTimeValue.Date };

            var dataList = new List<Entity>
            {
                new() { DateTimeValue = DateTime.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/12/2021 2:27:45 PM") },
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterLessThanOrEqual_TwoFitDateTimeNullableValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM");
            var filter = new FilterLessThanOrEqualNullableDateTime { DateTimeNullableValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeNullable = null },
                new() { DateTimeNullable = DateTime.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterLessThanOrEqual_FilterParameterIsNull_ReturnTheSameList()
        {
            var filter = new FilterLessThanOrEqualNullableDateTime { DateTimeNullableValue = null };

            var dataList = new List<Entity>
            {
                new() { DateTimeNullable = null },
                new() { DateTimeNullable = DateTime.Parse("11/10/2021 2:27:45 PM") },
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().And.BeEquivalentTo(dataList);
        }

        [Fact]
        public void FilterLessThanOrEqual_TwoFitDateTimeOffsetNullableValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTimeOffset.Parse("11/11/2021 2:27:45 PM");
            var filter = new FilterLessThanOrEqualNullableDateTimeOffset { DateTimeOffsetNullableValue = dateTimeValue };

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
