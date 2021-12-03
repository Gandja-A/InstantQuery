using System;
using InstantQuery.Attributes;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterNotEqualInt
    {
        [NotEqual(nameof(Entity.IntValue))]
        public int IntValue { get; set; }
    }

    public class FilterNotEqualString
    {
        [NotEqual(nameof(Entity.StringValue))]
        public string StringValue { get; set; }
    }

    public class FilterNotEqualNullableLong
    {
        [NotEqual(nameof(Entity.LongNullableValue))]
        public long? LongNullableValue { get; set; }
    }

    public class FilterNotEqualDateTime
    {
        [NotEqual(nameof(Entity.DateTimeValue))]
        public DateTime DateTimeValue { get; set; }
    }

    public class FilterNotEqualNullableDateTime
    {
        [NotEqual(For = nameof(Entity.DateTimeNullable))]
        public DateTime? DateTimeNullableValue { get; set; }
    }

    public class FilterNotEqualNullableDateTimeOffset
    {
        [NotEqual]
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }
    }
}
