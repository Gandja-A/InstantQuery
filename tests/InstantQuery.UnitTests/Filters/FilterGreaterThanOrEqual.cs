using System;
using InstantQuery.Attributes;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterGreaterThanOrEqualInt
    {
        [GreaterThanOrEqual(nameof(Entity.IntValue))]
        public int IntValue { get; set; }
    }

    public class FilterGreaterThanOrEqualString
    {
        [GreaterThanOrEqual(nameof(Entity.StringValue))]
        public string StringValue { get; set; }
    }

    public class FilterGreaterThanOrEqualNullableLong
    {
        [GreaterThanOrEqual(nameof(Entity.LongNullableValue))]
        public long? LongNullableValue { get; set; }
    }

    public class FilterGreaterThanOrEqualDateTime
    {
        [GreaterThanOrEqual(nameof(Entity.DateTimeValue))]
        public DateTime DateTimeValue { get; set; }
    }

    public class FilterGreaterThanOrEqualNullableDateTime
    {
        [GreaterThanOrEqual(For = nameof(Entity.DateTimeNullable))]
        public DateTime? DateTimeNullableValue { get; set; }
    }

    public class FilterGreaterThanOrEqualDateNullableTimeOffset
    {
        [GreaterThanOrEqual]
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }
    }
}
