using System;
using InstantQuery.Attributes;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterGreaterThanInt
    {
        [GreaterThan(nameof(Entity.IntValue))]
        public int IntValue { get; set; }
    }

    public class FilterGreaterThanString
    {
        [GreaterThan(nameof(Entity.StringValue))]
        public string StringValue { get; set; }
    }

    public class FilterGreaterThanNullableLong
    {
        [GreaterThan(nameof(Entity.LongNullableValue))]
        public long? LongNullableValue { get; set; }
    }

    public class FilterGreaterThanDateTime
    {
        [GreaterThan(nameof(Entity.DateTimeValue))]
        public DateTime DateTimeValue { get; set; }
    }

    public class FilterGreaterThanNullableDateTime
    {
        [GreaterThan(For = nameof(Entity.DateTimeNullable))]
        public DateTime? DateTimeNullableValue { get; set; }
    }

    public class FilterGreaterThanNullableDateTimeOffset
    {
        [GreaterThan]
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }
    }
}
