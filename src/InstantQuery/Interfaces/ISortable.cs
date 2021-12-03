namespace InstantQuery.Interfaces
{
    public interface ISortable
    {
        string SortBy { get; set; }

        string SortDir { get; set; }
    }
}
