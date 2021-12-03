using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.Filters;
using Xunit;

namespace InstantQuery.UnitTests
{
    public class LessThanFilterTests
    {
        [Fact]
        public void FilterLessThan_TwoFitIntValues_ReturnListOfTwoElements()
        {
            var filter = new FilterLessThanInt { IntValue = 2 };

            var dataList = new List<Entity> { new() { IntValue = 4 }, new() { IntValue = 0 }, new() { IntValue = 1 } }
                .AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterLessThan_TwoFitStringValues_ReturnListOfTwoElements()
        {
            var filter = new FilterLessThanString { StringValue = "c" };

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
        public void FilterLessThan_OneFitIntNullableValue_ReturnListOfOneElement()
        {
            var filter = new FilterLessThanNullableLong { LongNullableValue = 3 };

            var dataList = new List<Entity>
            {
                new() { LongNullableValue = null },
                new() { LongNullableValue = 1 },
                new() { LongNullableValue = 3 },
                new() { LongNullableValue = 4 },
                new() { LongNullableValue = 4 }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(1);
        }

        [Fact]
        public void FilterLessThan_OneFitDateTimeValue_ReturnListOfOneElement()
        {
            var dateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM");
            var filter = new FilterLessThanDateTime { DateTimeValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeValue = DateTime.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(1);
        }

        [Fact]
        public void FilterLessThan_OneFitDateTimeNullableValue_ReturnListOfOneElement()
        {
            var dateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM");
            var filter = new FilterLessThanNullableDateTime { DateTimeNullableValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeNullable = null },
                new() { DateTimeNullable = DateTime.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(1);
        }

        [Fact]
        public void FilterLessThan_FilterParameterIsNull_ReturnTheSameList()
        {
            var filter = new FilterLessThanNullableDateTime { DateTimeNullableValue = null };

            var dataList = new List<Entity>
            {
                new() { DateTimeNullable = null }, new() { DateTimeNullable = DateTime.Parse("11/10/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(filter).ToList();

            result.Should().BeOfType<List<Entity>>().And.BeEquivalentTo(dataList);
        }

        [Fact]
        public void FilterLessThan_TwoFitDateTimeOffsetNullableValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTimeOffset.Parse("11/12/2021 2:27:45 PM");
            var filter = new FilterLessThanNullableDateTimeOffset { DateTimeOffsetNullableValue = dateTimeValue };

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
