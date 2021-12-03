namespace InstantQuery.Examples.Common
{
    public class ListResultDto<T>
    {
        public IList<T> Data { get; set; }

        public int TotalCount { get; set; }
    }
}
