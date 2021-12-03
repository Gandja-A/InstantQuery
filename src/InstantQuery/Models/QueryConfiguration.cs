using System;
using InstantQuery.Attributes;

namespace InstantQuery.Models
{
    internal class QueryConfiguration
    {
        public QueryType QueryType { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public object Value { get; set; }

        public ComparisonType? CompareAs { get; set; }

        public CombineType CombineAs { get; set; }

        public SearchAs? SearchType { get; set; }

        public ComparisonRestriction ComparisonRestriction { get; set; } = ComparisonRestriction.None;
    }
}
