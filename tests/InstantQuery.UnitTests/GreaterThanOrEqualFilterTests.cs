using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.Filters;
using Xunit;

namespace InstantQuery.UnitTests
{
    public class GreaterThanOrEqualFilterTests
    {
        [Fact]
        public void FilterGreaterThanOrEqual_TwoFitIntValues_ReturnListOfTwoElements()
        {
            var queryModel = new FilterGreaterThanOrEqualInt { IntValue = 2 };

            var dataList = new List<Entity>
            {
                new() { IntValue = 0 }, new() { IntValue = 1 }, new() { IntValue = 2 }, new() { IntValue = 5 }
            }.AsQueryable();

            var result = dataList.Filter(queryModel).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterGreaterThanOrEqual_TwoFitStringValues_ReturnListOfTwoElements()
        {
            var queryModel = new FilterGreaterThanOrEqualString { StringValue = "b" };

            var dataList = new List<Entity>
            {
                new() { IntValue = 0, StringValue = "A" },
                new() { IntValue = 1, StringValue = "A" },
                new() { IntValue = 2, StringValue = "b" },
                new() { IntValue = 5, StringValue = "c" }
            }.AsQueryable();

            var result = dataList.Filter(queryModel).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterGreaterThanOrEqual_TwoFitIntNullableValues_ReturnListOfTwoElements()
        {
            var queryModel = new FilterGreaterThanOrEqualNullableLong { LongNullableValue = 3 };

            var dataList = new List<Entity>
            {
                new() { LongNullableValue = null },
                new() { LongNullableValue = 1 },
                new() { LongNullableValue = 3 },
                new() { LongNullableValue = 4 }
            }.AsQueryable();

            var result = dataList.Filter(queryModel).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterGreaterThanOrEqual_TwoFitDateTimeValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM");
            var queryModel = new FilterGreaterThanOrEqualDateTime { DateTimeValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeValue = DateTime.Parse("11/9/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeValue = DateTime.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(queryModel).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterGreaterThanOrEqual_TwoFitDateTimeNullableValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTime.Parse("11/11/2021 2:27:45 PM");
            var queryModel = new FilterGreaterThanOrEqualNullableDateTime { DateTimeNullableValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeNullable = null },
                new() { DateTimeNullable = DateTime.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(queryModel).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterGreaterThanOrEqual_FilterParameterIsNull_OneFitDateTimeNullable_ReturnTheSameList()
        {
            var queryModel = new FilterGreaterThanOrEqualNullableDateTime { DateTimeNullableValue = null };

            var dataList = new List<Entity>
            {
                new() { DateTimeNullable = null },
                new() { DateTimeNullable = DateTime.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeNullable = DateTime.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(queryModel).ToList();

            result.Should().BeOfType<List<Entity>>().And.BeEquivalentTo(dataList);
        }

        [Fact]
        public void FilterGreaterThanOrEqual_TwoFitDateTimeOffsetNullableValues_ReturnListOfTwoElements()
        {
            var dateTimeValue = DateTimeOffset.Parse("11/11/2021 2:27:45 PM");
            var queryModel =
                new FilterGreaterThanOrEqualDateNullableTimeOffset { DateTimeOffsetNullableValue = dateTimeValue };

            var dataList = new List<Entity>
            {
                new() { DateTimeOffsetNullableValue = null },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(queryModel).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public void FilterGreaterThanOrEqual_FilterParameterIsNull_OneFitDateTimeOffsetNullable_ReturnTheSameList()
        {
            var queryModel = new FilterGreaterThanOrEqualDateNullableTimeOffset { DateTimeOffsetNullableValue = null };

            var dataList = new List<Entity>
            {
                new() { DateTimeOffsetNullableValue = null },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/10/2021 2:27:45 PM") },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/11/2021 2:27:45 PM") },
                new() { DateTimeOffsetNullableValue = DateTimeOffset.Parse("11/12/2021 2:27:45 PM") }
            }.AsQueryable();

            var result = dataList.Filter(queryModel).ToList();

            result.Should().BeOfType<List<Entity>>().And.BeEquivalentTo(dataList);
        }
    }
}
