using InstantQuery.Interfaces;
using InstantQuery.Models;
using Microsoft.EntityFrameworkCore;

namespace InstantQuery.Examples
{
    public static class QueryableExtensions
    {
        public static async Task<ListResult<T>> ToListResultAsync<T, TFilter>(this IQueryable<T> query, TFilter queryParams)
            where TFilter : IPaging, ISortable
        {
            var filteredAndSortedQuery = query.FilterAndSort(queryParams);

            var totalCount = await filteredAndSortedQuery.CountAsync();

            var data = await filteredAndSortedQuery.TakePage(queryParams).ToListAsync();

            return new ListResult<T> { Data = data, TotalCount = totalCount };
        }

    }
}
