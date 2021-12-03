using System;

namespace InstantQuery.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainsAttribute : QueryAttribute
    {
        public ContainsAttribute()
        {
        }

        public ContainsAttribute(string forMember, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
        }
    }
}
