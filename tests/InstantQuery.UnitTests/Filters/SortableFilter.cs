using InstantQuery.Interfaces;

namespace InstantQuery.UnitTests.Filters
{
    public class SortableFilter : ISortable
    {
        public string SortBy { get; set; }
        public string SortDir { get; set; }
    }
}
