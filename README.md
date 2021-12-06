# Instant Query

The tiny library intended to simplify writing LINQ queries based on attributes annotation to the database. 

The primary purpose is to reduce repeatable and annoying code like below during writing queries using Entity Framework Core and LINQ.

```csharp
if(filter.StartDate != null && filter.EndDate != null)
{
    query = query.Where(o => o.CreatedAt.Date >= filter.StartDate && o.CreatedAt.Date <= filter.EndDate);
}

if(filter.StartDate != null && filter.EndDate == null)
{
    query = query.Where(o => o.CreatedAt.Date >= filter.StartDate);
}

```

### Installing InstantQuery

    Install-Package InstantQuery -Version 1.0.0-rc

Usage
-----
To implement filtering, ordering, and pagination, you should decorate your filter DTO with query attributes and implement two interfaces.

### The Example of filter model

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
And then, you can use the ToListResult extension method.


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

That's It.

Here is the equivalent vanilla LINQ implementation without AutoMapper ProjectTo queryable extension method.

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
This library does not provide an asynchronous method ToListResultAsync out of the box. You should implement it by yourself. 
```csharp
public static class QueryableExtensions
{
    public static async Task<ListResult<T>> ToListResultAsync<T, TFilter>(this <T> query, TFilter queryParams)
    where TFilter : IPagination, ISortable
    {
        var filteredAndSortedQuery = query.FilterAndSort(queryParams);

        var totalCount = await filteredAndSortedQuery.CountAsync();

        var data = await filteredAndSortedQuery.TakePage(queryParams).ToListAsync();

        return new ListResult<T> { Data = data, TotalCount = totalCount };
    }

}

```

For more details, look at the demo project. The project gives an overview of using this library with EF Core 6 and an Angular application. For simplicity, the project is built based on the "ASP .NET Core with Angular" Visual Studio template.

## API

### Attributes parameters
| Parameter 	| Type 	| Description 	|
|---	|---	|---	|
| <b>For</b> 	| `string` optional 	| Uses for mapping filter property to the projections entity field. If not specified, the filter property name will use for mapping. 	|
| <b>CombineType</b> 	| `enum CombineType` <br> <br><i>Values:</i> CombineType.And, CombineType.Or<br><br><i>Default value:</i> CombineType.And 	| Defines how expressions are combined. 	|
| <b>SearchAs</b> 	| `enum SearchAs` <br> <br><i>Values:</i> SearchAs.StartsWith, SearchAs.Contains <br> <br><i>Default value:</i> SearchAs.StartsWith 	| Defines which string method uses to generate the search expression. 	|                                                                                 |
### Attributes

| Attribute 	| Equivalent LINQ expression 	|
|---	|---	|
| Equal 	| query.Where(e => e.EntityField == filter.Value); 	|
| NotEqual 	| query.Where(e => e.EntityField != filter.Value); 	|
| GreaterThan 	| query.Where(e => e.EntityField > filter.Value); 	|
| GreaterThanOrEqua 	| query.Where(e => e.EntityField >= filter.Value); 	|
| LessThan 	| query.Where(e => e.EntityField < filter.Value); 	|
| LessThanOrEqual 	| query.Where(e => e.EntityField <= filter.Value); 	|
| Contains 	| query.Where(e => filter.Values.Contains(e.EntityField)); 	|
| SearchBy 	| query = query.Where(q => q.EntityField.ToLower().StartsWith(filter.SearchTerm)); 	|
| CompareIgnoreTime 	| query = query.Where(q => q.EntityDateTimeField.Date ( any of comparison operator) filter.DateTimeValue)); 	|
### Queryable Extensions

| Methode 	| Descriptions 	|
|---	|---	|
| Filter 	| Generates filtering expression based on the attributes annotation. 	|
| FilterAndSort 	| Generates filtering and sorting expression based on the attribute annotation. 	|
| TakePage 	| Applies pagination based on the IPagination interface implementation. Equivalent to LINQ query.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize); 	|
| Sort 	| Generates sorting expression based on the ISortable interface implementation. 	|
| OrderBy 	| Sorts the elements based on the string input parameters. 	|
| ToListResult 	| Applies the Filtering, Sorting, and pagination. Evaluates a query, and returns the result as a ``` ListResult<T> ``` object. 	|

### Interfaces

<b>ISortable</b>

``` csharp
public interface ISortable
{
    string SortBy { get; set; }

    string SortDir { get; set; }
}
```
<b>Parameters</b> 
* `SortBy` - represents the target entity property/properties as a string with "," as a delimiter.
* `SortDir` - represents the direction of sorting as a string with "," as a delimiter. The parameter can be "asc" or "desc"  or a combination of them.

<b>Examples </b> <br>
SortBy = "entityField1, entityField2"<br>
SortDir ="asc, desc"

This configuration translates to the expression that is equivalent to the LINQ. 

```csharp
query = query.OrderBy(e=>e.EntityField1).ThenByDescending(e=>e.EntityField2)

```
SortBy = "entityField1, entityField2"<br>
SortDir ="asc"

```csharp
query = query.OrderBy(e=>e.EntityField1).ThenBy(e=>e.EntityField2)
```

```csharp
query = query.OrderBy(e=>e.EntityField1).ThenByDescending(e=>e.EntityField2)

```
SortBy = "entityField1"<br>
SortDir ="asc"

```csharp
query = query.OrderBy(e=>e.EntityField1)
```

<b>IPaging</b>

``` csharp
public interface IPaging
{
    int Page { get; set; }

    int PageSize { get; set; }
}

```

<b>Parameters</b> 
* `Page` - number of the requested page. Must be greater than zero. 
* `PageSize` - number of the retrieving records. Must be greater than zero.