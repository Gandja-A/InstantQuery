using System;

namespace InstantQuery.Attributes
{

    public enum ComparisonRestriction
    {
        None = 0,
        IgnoreTime = 1,
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ComparisonRestrictionAttribute : BaseQueryAttribute
    {
        public ComparisonRestriction Restriction { get; set; }
    }

    public class CompareIgnoreTimeAttribute : ComparisonRestrictionAttribute
    {
        public CompareIgnoreTimeAttribute()
        {
            this.Restriction = ComparisonRestriction.IgnoreTime;
        }
    }
}
