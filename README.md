# Instant Query

Instant Query is a small library designed to simplify the process of writing LINQ queries by using attribute annotations, particularly when working with Entity Framework Core and LINQ. Its main goal is to reduce the repetitive and tedious coding aspects often encountered during query construction.

Consider the following typical code snippet that Instant Query aims to streamline:

```csharp
if(filter.StartDate != null && filter.EndDate != null)
{
    query = query.Where(o => o.CreatedAt.Date >= filter.StartDate && o.CreatedAt.Date <= filter.EndDate);
}

if(filter.StartDate != null && filter.EndDate == null)
{
    query = query.Where(o => o.CreatedAt.Date >= filter.StartDate);
}

if(filter.StartDate == null && filter.EndDate != null)
{
    query = query.Where(o => o.CreatedAt.Date <= filter.EndDate);
}
```

### Installing InstantQuery

To install InstantQuery, use the following command:

    Install-Package InstantQuery

## Usage

InstantQuery facilitates the implementation of filtering, ordering, and pagination in your applications. To leverage these features, decorate your filter DTO (Data Transfer Object) with query attributes and implement the `IPaging` and `ISortable` interfaces.

### Example of a Filter Model

Here is an example of how a filter model might be defined using InstantQuery:

```csharp
public class OrderFilterDto : IPaging, ISortable
{
    [SearchBy(nameof(OrderDetailsDto.UserFullName))]
    public string SearchTerm { get; set; }

    [GreaterThan(nameof(OrderDetailsDto.CreatedAt))]
    [CompareIgnoreTime]
    public DateTime? StartDate { get; set; }

    [LessThanOrEqual(nameof(OrderDetailsDto.CreatedAt))]
    [CompareIgnoreTime]
    public DateTime? EndDate { get; set; }

    [Contains(nameof(OrderDetailsDto.OrderStatusId))]
    public List<long> StatusIds { get; set; }

    public int Page { get; set; }
    
    public int PageSize { get; set; }

    public string SortBy { get; set; }

    public string SortDir { get; set; }
}
```

### Using the Extension Method

The library provides an extension method, `ToListResult`, to apply these annotations in your queries:

```csharp
[HttpGet]
[ProducesResponseType(typeof(ListResultDto<OrderDetailsDto>), StatusCodes.Status200OK)]
public async Task<ActionResult<ListResultDto<OrderDetailsDto>>> GetOrders([FromQuery] OrderFilterDto filter)
{
    ListResult<OrderDetailsDto> orderDetails = await this.dbContext.Orders
        .ProjectTo<OrderDetailsDto>(this.mapper.ConfigurationProvider)
        .ToListResultAsync(filter);

    var orderDetailsRes = this.mapper.Map<ListResultDto<OrderDetailsDto>>(orderDetails);
    return this.Ok(orderDetailsRes);
}
```

## Vanilla LINQ Implementation

The equivalent implementation using vanilla LINQ, without the AutoMapper's ProjectTo extension method, is as follows:

```csharp
var columnsMap = new Dictionary<string, Expression<Func<OrderDetailsDto, object>>>
{
    ["statusName"] = v => v.StatusName,
    ["createdAt"] = v => v.CreatedAt,
    ["userFullName"] = v => v.UserFullName,
    ["statusName"] = v => v.StatusName,
    ["quantity"] = v => v.Quantity,
    ["lotNumber"] = v => v.LotNumber
};

var query = this.dbContext.Orders.Select(o => new OrderDetailsDto
{
    UserFullName = o.User.FirstName + " " + o.User.LastName,
    CreatedAt = o.CreatedAt,
    StatusName = o.OrderStatus.Name,
    OrderStatusId = o.OrderStatusId,
    Item = o.Item,
    LotNumber = o.LotNumber,
    OrderId = o.Id,
    Quantity = o.Quantity,
    UserId = o.UserId
});

if(filter.StartDate != null && filter.EndDate != null)
{
    query = query.Where(o => o.CreatedAt.Date >= filter.StartDate && o.CreatedAt.Date <= filter.EndDate);
}

if(filter.StartDate != null && filter.EndDate == null)
{
    query = query.Where(o => o.CreatedAt.Date >= filter.StartDate);
}

if(filter.StartDate != null && filter.EndDate == null)
{
    query = query.Where(o => o.CreatedAt.Date <= filter.EndDate);
}

if(!string.IsNullOrWhiteSpace(filter.SearchTerm))
{
    query = query.Where(q => q.UserFullName.ToLower().StartsWith(filter.SearchTerm.ToLower()));
}

if(filter.StatusIds.Any())
{
    query = query.Where(q => filter.StatusIds.Contains(q.OrderStatusId));
}

if(!string.IsNullOrWhiteSpace(filter.SortBy) && columnsMap.ContainsKey(filter.SortBy))
{
    var sortDir = !string.IsNullOrWhiteSpace(filter.SortDir) ? filter.SortDir : "asc";

    if(sortDir == "asc")
    {
        query = query.OrderBy(columnsMap[filter.SortBy]);
    }
    else
    {
        query = query.OrderByDescending(columnsMap[filter.SortBy]);
    }
}

var totalCount = await query.CountAsync();

if(filter.PageSize <= 0)
{
    return this.BadRequest("PageSize must be > 0");
}

if(filter.Page <= 0)
{
    return this.BadRequest("Page must be > 0");
}

var orders = await query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

var result = new ListResultDto<OrderDetailsDto> { Data = orders, TotalCount = totalCount };
```

### Note
InstantQuery does not provide the method `ToListResultAsync` by default. You need to implement it as shown below:

```csharp
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
```

For more detailed examples

 and usage scenarios, refer to the demo project. This project provides a comprehensive guide on how to use InstantQuery with EF Core 6 and an Angular application, structured around the "ASP .NET Core with Angular" Visual Studio template.

## API Documentation

### Attribute Parameters

| Parameter       | Type                            | Description |
|-----------------|---------------------------------|-------------|
| `For`           | `string` (optional)             | Maps filter property to the entity field. If not set, the filter property name is used. |
| `CombineType`   | `enum CombineType`              | Specifies how expressions are combined. Default: `CombineType.And` |
| `SearchAs`      | `enum SearchAs`                 | Determines the string method for search expression. Default: `SearchAs.StartsWith` |

### Attributes

| Attribute           | Equivalent LINQ Expression |
|---------------------|----------------------------|
| `Equal`             | `query.Where(e => e.EntityField == filter.Value);` |
| `NotEqual`          | `query.Where(e => e.EntityField != filter.Value);` |
| `GreaterThan`       | `query.Where(e => e.EntityField > filter.Value);` |
| `GreaterThanOrEqual`| `query.Where(e => e.EntityField >= filter.Value);` |
| `LessThan`          | `query.Where(e => e.EntityField < filter.Value);` |
| `LessThanOrEqual`   | `query.Where(e => e.EntityField <= filter.Value);` |
| `Contains`          | `query.Where(e => filter.Values.Contains(e.EntityField));` |
| `SearchBy`          | `query.Where(q => q.EntityField.ToLower().StartsWith(filter.SearchTerm.ToLower()));` |
| `CompareIgnoreTime` | `query.Where(q => q.EntityDateTimeField.Date (any comparison operator) filter.DateTimeValue));` |

### Queryable Extensions

| Method            | Description |
|-------------------|-------------|
| `Filter`          | Generates filter expressions based on annotations. |
| `FilterAndSort`   | Combines filtering and sorting expressions. |
| `TakePage`        | Applies pagination as per `IPagination`. |
| `Sort`            | Creates sort expressions based on `ISortable`. |
| `OrderBy`         | Sorts elements based on string parameters. |
| `ToListResult`    | Applies filtering, sorting, pagination, and returns `ListResult<T>`. |

### Interfaces

**`ISortable`**
- `SortBy`: Target entity properties as a comma-separated string.
- `SortDir`: Sorting direction ("asc", "desc", or combinations).

**`IPaging`**
- `Page`: Requested page number, must be > 0.
- `PageSize`: Number of records to retrieve, must be > 0.
