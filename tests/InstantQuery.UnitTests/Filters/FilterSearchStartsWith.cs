using InstantQuery.Attributes;
using InstantQuery.UnitTests.Entities;

namespace InstantQuery.UnitTests.Filters
{
    public class FilterSearchStartsWith
    {
        [SearchBy(nameof(Entity.StringValue))]
        public string StringValue { get; set; }
    }

    public class FilterSearchContains
    {
        [SearchBy(nameof(Entity.StringValue), SearchAs.Contains)]
        public string StringValue { get; set; }
    }
}
