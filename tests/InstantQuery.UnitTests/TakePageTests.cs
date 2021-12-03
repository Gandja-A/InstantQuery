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
    public class TakePageTests : IDisposable
    {
        private readonly TestDbContext dbContext;

        public TakePageTests()
        {
            this.dbContext = TestDbContext.CreateContext();
            this.dbContext.AddFiveEntities();
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        [Fact]
        public void TakePage_TakePageStartFromZeroSize3_ReturnListOfThreeElements()
        {
            var paginationFilter = new PagingFilter { Page = 1, PageSize = 3 };

            var result = this.dbContext.Entities.OrderBy(e => e.Id).TakePage(paginationFilter).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(3);
        }

        [Fact]
        public void TakePage_ZeroPage_ShouldThrowAnException()
        {
            var paginationFilter = new PagingFilter { Page = 0, PageSize = 1 };

            Assert.Throws<ArgumentException>(() =>
                this.dbContext.Entities.OrderBy(e => e.Id).TakePage(paginationFilter).ToList());
        }

        [Fact]
        public void TakePage_ZeroPageSize_ShouldThrowAnException()
        {
            var paginationFilter = new PagingFilter { Page = 0, PageSize = 0 };

            Assert.Throws<ArgumentException>(() =>
                this.dbContext.Entities.OrderBy(e => e.Id).TakePage(paginationFilter).ToList());
        }
    }
}
