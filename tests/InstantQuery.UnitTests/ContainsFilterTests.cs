using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantQuery.UnitTests.Entities;
using InstantQuery.UnitTests.Filters;
using InstantQuery.UnitTests.MockData;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace InstantQuery.UnitTests
{
    public class ContainsFilterTests : IDisposable
    {
        private readonly TestDbContext dbContext;

        private readonly ITestOutputHelper output;

        public ContainsFilterTests(ITestOutputHelper output)
        {
            this.output = output;
            this.dbContext = TestDbContext.CreateContext();
            this.dbContext.AddFiveEntities();
        }

        [Fact]
        public async Task Filter_ValuesOfLongTypeAsAQueryParamsTwoFitValues_ReturnListOfTwoElements()
        {
            var queryModel = new FilterContainsLongValues { StatusIds = new List<long> { 1 } };
            var sw = new Stopwatch();
            sw.Start();

            var result = await this.dbContext.Entities.Filter(queryModel).ToListAsync();
            var e1 = sw.Elapsed;
            this.output.WriteLine("This is output from {0}", e1);

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        [Fact]
        public async Task
            Filter_ValuesOfLongTypeAsAQueryParamsMapToNullablePropertyOneFitValue_ReturnListOfOneElements()
        {
            var queryModel = new FilterContainsLongNullableValues { ValueIds = new List<long> { 1 } };

            var result = await this.dbContext.Entities.Filter(queryModel).ToListAsync();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(1);
        }

        public void Test()
        {
            var dataList = new List<Entity>
            {
                new() { IntValue = 1, StringValue = "A" },
                new() { IntValue = 2, StringValue = "A" },
                new() { IntValue = 5, StringValue = "A" }
            }.AsQueryable();

            // var result = dataList.Where(c => c.City == "A" || c.Age == 5 && c.Age == 1).ToList();
            var result = dataList.Where(c => c.IntValue == 5 || (c.IntValue == 1 && c.StringValue == "A")).ToList();

            result.Should().BeOfType<List<Entity>>().Which.Count.Should().Be(2);
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }
    }
}
