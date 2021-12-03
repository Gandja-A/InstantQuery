using System.Collections.Generic;
using FluentAssertions;
using InstantQuery.Interfaces;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.MockData;
using Xunit;

namespace InstantQuery.UnitTests
{
    internal class ResultListFilter : ISortable, IPaging
    {
        public string SortBy { get; set; }
        public string SortDir { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ListResultTests
    {
        private readonly TestDbContext dbContext;

        public ListResultTests()
        {
            this.dbContext = TestDbContext.CreateContext();
            this.dbContext.AddFiveEntities();
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        [Fact]
        public void ListResult_TakePageStartFromZeroSizeThree_ReturnListOfThreeElements()
        {
            var filter = new ResultListFilter { SortBy = "Id", SortDir = "desc", Page = 1, PageSize = 3 };

            var result = this.dbContext.Entities.ToListResult(filter);

            result.Data.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(3);
            result.TotalCount.Should().Be(5);
            result.Data.Should().BeInDescendingOrder(r => r.Id);
        }
    }
}
