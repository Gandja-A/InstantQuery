using System;
using InstantQuery.Attributes;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterEqualInt
    {
        [Equal(nameof(Entity.IntValue))]
        public int IntValue { get; set; }
    }

    public class FilterEqualString
    {
        [Equal(nameof(Entity.StringValue))]
        public string StringValue { get; set; }
    }

    public class FilterEqualNullableLong
    {
        [Equal(nameof(Entity.LongNullableValue))]
        public long? LongNullableValue { get; set; }
    }

    public class FilterEqualDateTime
    {
        [Equal(nameof(Entity.DateTimeValue))]
        public DateTime DateTimeValue { get; set; }
    }

    public class FilterEqualNullableDateTime
    {
        [Equal(For = nameof(Entity.DateTimeNullable))]
        public DateTime? DateTimeNullableValue { get; set; }
    }

    public class FilterEqualNullableDateTimeOffset
    {
        [Equal]
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }
    }
}
