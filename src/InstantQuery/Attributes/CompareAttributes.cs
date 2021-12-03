using System;

namespace InstantQuery.Attributes
{
    public enum ComparisonType
    {
        Equal = 0,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CompareAttribute : QueryAttribute
    {
        public ComparisonType CompareAs { get; set; }

        public CompareAttribute()
        {
        }

        public CompareAttribute(string forMember, ComparisonType compareAs, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
            this.CompareAs = compareAs;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterThanOrEqualAttribute : CompareAttribute
    {
        public GreaterThanOrEqualAttribute()
        {
            this.CompareAs = ComparisonType.GreaterThanOrEqual;
        }

        public GreaterThanOrEqualAttribute(string forMember, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
            this.CompareAs = ComparisonType.GreaterThanOrEqual;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotEqualAttribute : CompareAttribute
    {
        public NotEqualAttribute()
        {
            this.CompareAs = ComparisonType.NotEqual;
        }

        public NotEqualAttribute(string forMember, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
            this.CompareAs = ComparisonType.NotEqual;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class EqualAttribute : CompareAttribute
    {
        public EqualAttribute()
        {
            this.CompareAs = ComparisonType.Equal;
        }

        public EqualAttribute(string forMember, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
            this.CompareAs = ComparisonType.Equal;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterThanAttribute : CompareAttribute
    {
        public GreaterThanAttribute()
        {
            this.CompareAs = ComparisonType.GreaterThan;
        }

        public GreaterThanAttribute(string forMember, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
            this.CompareAs = ComparisonType.GreaterThan;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LessThanAttribute : CompareAttribute
    {
        public LessThanAttribute()
        {
            this.CompareAs = ComparisonType.LessThan;
        }

        public LessThanAttribute(string forMember, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
            this.CompareAs = ComparisonType.LessThan;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LessThanOrEqualAttribute : CompareAttribute
    {
        public LessThanOrEqualAttribute()
        {
            this.CompareAs = ComparisonType.LessThanOrEqual;
        }

        public LessThanOrEqualAttribute(string forMember, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
            this.CompareAs = ComparisonType.LessThanOrEqual;
        }
    }
}
