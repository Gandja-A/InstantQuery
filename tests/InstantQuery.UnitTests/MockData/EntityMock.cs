using System;
using System.Collections.Generic;
using System.Linq;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.MockData
{
    public static class EntityMock
    {
        public static List<Entity> AddEntities(this TestDbContext context, params Entity[] customers)
        {
            context.Entities.AddRange(customers);
            context.SaveChanges();
            return customers.ToList();
        }

        public static List<Entity> AddFiveEntities(this TestDbContext context)
        {
            var customers = CreateFiveCustomers();
            context.Entities.AddRange(customers);
            context.SaveChanges();
            return customers.ToList();
        }

        private static List<Entity> CreateFiveCustomers()
        {
            var customers = new List<Entity>
            {
                new()
                {
                    Id = 5,
                    StringValue = "2 Terri Lee Duffy",
                    IntValue = 30,
                    DateTimeValue = DateTime.Parse("01/01/2021"),
                    GuidValue = Guid.Parse("E6560E7C-529A-41F8-A4FE-E489F701F525"),
                    DateTimeOffsetValue = DateTimeOffset.Parse("01/02/2021 6:00:00AM +5:00"),
                    DateTimeNullable = DateTime.Parse("10/10/2021"),
                    LongValue = 1,
                    LongNullableValue = 1
                },
                new()
                {
                    Id = 4,
                    StringValue = "Terri Lee Duffy",
                    IntValue = 40,
                    DateTimeValue = DateTime.Parse("01/01/2021"),
                    GuidValue = Guid.Parse("E6560E7C-529A-41F8-A4FE-E489F701F525"),
                    DateTimeOffsetValue = DateTimeOffset.Parse("01/02/2021 6:00:00AM +5:00"),
                    DateTimeNullable = DateTime.Parse("10/10/2021"),
                    LongValue = 1
                },
                new()
                {
                    Id = 3,
                    StringValue = "Janice Galvin",
                    IntValue = 60,
                    DateTimeValue = DateTime.Parse("03/03/2021"),
                    GuidValue = Guid.Parse("E6560E7C-529A-41F8-A4FE-E489F701F525"),
                    DateTimeOffsetValue = DateTimeOffset.Parse("01/02/2021 7:00:00AM +5:00"),
                    DateTimeNullable = DateTime.Parse("10/11/2021"),
                    LongValue = 2
                },
                new()
                {
                    Id = 2,
                    StringValue = "Michael Raheem",
                    IntValue = 55,
                    DateTimeValue = DateTime.Parse("04/04/2021"),
                    GuidValue = Guid.Parse("E6560E7C-529A-41F8-A4FE-E489F701F525"),
                    DateTimeOffsetValue = DateTimeOffset.Parse("01/02/2021 8:00:00AM +5:00"),
                    DateTimeNullable = DateTime.Parse("05/05/2021"),
                    LongValue = 2
                },
                new()
                {
                    Id = 1,
                    StringValue = "2",
                    IntValue = 18,
                    DateTimeValue = DateTime.Parse("05/05/2021"),
                    GuidValue = Guid.Parse("E6560E7C-529A-41F8-A4FE-E489F701F525"),
                    DateTimeOffsetValue = DateTimeOffset.Parse("01/02/2021 9:00:00AM +5:00"),
                    LongValue = 3
                }
            };
            return customers;
        }
    }
}
