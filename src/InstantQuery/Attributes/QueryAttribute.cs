using System;

namespace InstantQuery.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryAttribute : BaseQueryAttribute
    {
        public string For { get; set; }

        public CombineType CombineAs { get; set; } = CombineType.And;
    }
}
