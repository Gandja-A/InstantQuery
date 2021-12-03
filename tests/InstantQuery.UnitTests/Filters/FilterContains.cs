using System.Collections.Generic;
using InstantQuery.Attributes;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterContainsLongValues
    {
        [Contains(nameof(Entity.LongValue))]
        public List<long> StatusIds { get; set; }
    }

    public class FilterContainsLongNullableValues
    {
        [Contains(nameof(Entity.LongNullableValue))]
        public List<long> ValueIds { get; set; }
    }
}
