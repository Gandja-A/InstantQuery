using InstantQuery.Interfaces;

namespace InstantQuery.UnitTests.Filters
{
    public class PagingFilter : IPaging
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
