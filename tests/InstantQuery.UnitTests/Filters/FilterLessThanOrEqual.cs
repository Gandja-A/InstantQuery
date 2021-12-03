using System;
using InstantQuery.Attributes;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterLessThanOrEqualInt
    {
        [LessThanOrEqual(nameof(Entity.IntValue))]
        public int IntValue { get; set; }
    }

    public class FilterLessThanOrEqualString
    {
        [LessThanOrEqual(nameof(Entity.StringValue))]
        public string StringValue { get; set; }
    }

    public class FilterLessThanOrEqualNullableLong
    {
        [LessThanOrEqual(nameof(Entity.LongNullableValue))]
        public long? LongNullableValue { get; set; }
    }

    public class FilterLessThanOrEqualDateTime
    {
        [LessThanOrEqual(nameof(Entity.DateTimeValue))]
        [CompareIgnoreTime]
        public DateTime DateTimeValue { get; set; }
    }

    public class FilterLessThanOrEqualNullableDateTime
    {
        [LessThanOrEqual(For = nameof(Entity.DateTimeNullable))]
        public DateTime? DateTimeNullableValue { get; set; }
    }

    public class FilterLessThanOrEqualNullableDateTimeOffset
    {
        [LessThanOrEqual]
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }
    }
}
