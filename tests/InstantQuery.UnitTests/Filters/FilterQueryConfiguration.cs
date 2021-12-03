using System;
using InstantQuery.Attributes;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterQueryConfiguration
    {
        [Equal]
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }
    }

    public class FilterQueryConfTwoAttributeApplyToField
    {
        [Equal]
        [NotEqual]
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }
    }
}
