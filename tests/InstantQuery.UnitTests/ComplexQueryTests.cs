using System;
using System.Collections.Generic;
using FluentAssertions;
using InstantQuery.Attributes;
using InstantQuery.Interfaces;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.MockData;
using Xunit;

namespace InstantQuery.UnitTests
{
    internal class ComplexQueryFilter : IPaging, ISortable
    {
        [SearchBy(nameof(Entity.StringValue), SearchAs.StartsWith, CombineType.Or)]
        public string SearchPattern { get; set; }

        [GreaterThan(nameof(Entity.DateTimeValue))]
        public DateTime StartDate { get; set; }

        [LessThanOrEqual(nameof(Entity.DateTimeValue))]
        public DateTime EndDate { get; set; }

        [Compare(nameof(Entity.IntValue), ComparisonType.Equal, CombineType.Or)]
        public int IntValue { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string SortBy { get; set; }

        public string SortDir { get; set; }
    }

    public class ComplexQueryTests : IDisposable
    {
        private readonly TestDbContext dbContext;

        public ComplexQueryTests()
        {
            this.dbContext = TestDbContext.CreateContext();
            this.dbContext.AddFiveEntities();
        }

        [Fact]
        public void ListResult_ComplexQueryFilter_ReturnListOfTwoElements()
        {
            var filter = new ComplexQueryFilter
            {
                StartDate = DateTime.Parse("01/01/2021"),
                EndDate = DateTime.Parse("05/05/2021"),
                SearchPattern = "Michael",
                IntValue = 60,
                SortBy = "DateTimeOffsetValue",
                SortDir = "desc",
                Page = 1,
                PageSize = 3
            };

            var result = this.dbContext.Entities.ToListResult(filter);

            result.Data.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
            result.TotalCount.Should().Be(2);
            result.Data.Should().BeInDescendingOrder(r => r.DateTimeOffsetValue);
        }

        public void Dispose()
        {
            this.dbContext?.Dispose();
        }
    }
}
