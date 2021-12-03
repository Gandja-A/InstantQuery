using System;
using InstantQuery.Attributes;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterLessThanInt
    {
        [LessThan(nameof(Entity.IntValue))]
        public int IntValue { get; set; }
    }

    public class FilterLessThanString
    {
        [LessThan(nameof(Entity.StringValue))]
        public string StringValue { get; set; }
    }

    public class FilterLessThanNullableLong
    {
        [LessThan(nameof(Entity.LongNullableValue))]
        public long? LongNullableValue { get; set; }
    }

    public class FilterLessThanDateTime
    {
        [LessThan(nameof(Entity.DateTimeValue))]
        public DateTime DateTimeValue { get; set; }
    }

    public class FilterLessThanNullableDateTime
    {
        [LessThan(For = nameof(Entity.DateTimeNullable))]
        public DateTime? DateTimeNullableValue { get; set; }
    }

    public class FilterLessThanNullableDateTimeOffset
    {
        [LessThan]
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }
    }
}
