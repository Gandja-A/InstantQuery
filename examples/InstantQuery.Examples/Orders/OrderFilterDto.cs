using InstantQuery.Attributes;
using InstantQuery.Interfaces;

namespace InstantQuery.Examples.Orders
{
    public class OrderFilterDto : IPaging, ISortable
    {
        [SearchBy(nameof(OrderDetailsDto.UserFullName))]
        public string SearchTerm { get; set; }

        [GreaterThanOrEqual(nameof(OrderDetailsDto.CreatedAt))]
        [CompareIgnoreTime]
        public DateTime? StartDate { get; set; }

        [LessThanOrEqual(nameof(OrderDetailsDto.CreatedAt))]
        [CompareIgnoreTime]
        public DateTime? EndDate { get; set; }

        [Contains(nameof(OrderDetailsDto.OrderStatusId))]
        public List<long> StatusIds { get; set; } = new();

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDir { get; set; }
    }
}
