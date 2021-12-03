using System.Collections.Generic;

namespace InstantQuery.Models
{
    public class ListResult<T>
    {
        public IList<T> Data { get; set; }

        public int TotalCount { get; set; }
    }
}
