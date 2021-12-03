using System;

namespace InstantQuery.Attributes
{
    public enum SearchAs
    {
        StartsWith,
        Contains
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SearchByAttribute : QueryAttribute
    {
        public SearchAs SearchAs { get; }

        public SearchByAttribute(string forMember, SearchAs searchAs = SearchAs.StartsWith, CombineType combineAs = CombineType.And)
        {
            this.For = forMember;
            this.CombineAs = combineAs;
            this.SearchAs = searchAs;
        }
    }
}
