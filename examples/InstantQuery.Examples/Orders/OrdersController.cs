using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using InstantQuery.Examples.Common;
using InstantQuery.Examples.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstantQuery.Examples.Orders
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ExamplesDbContext dbContext;

        private readonly IMapper mapper;

        public OrdersController(ExamplesDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListResultDto<OrderDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ListResultDto<OrderDetailsDto>>> GetOrders([FromQuery] OrderFilterDto filter)
        {
            var orderDetails = await this.dbContext.Orders
                .ProjectTo<OrderDetailsDto>(this.mapper.ConfigurationProvider)
                .ToListResultAsync(filter);

            var orderDetailsRes = this.mapper.Map<ListResultDto<OrderDetailsDto>>(orderDetails);
            return this.Ok(orderDetailsRes);
        }

        [HttpGet("vanilla-linq")]
        [ProducesResponseType(typeof(ListResultDto<OrderDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ListResultDto<OrderDetailsDto>>> GetOrdersVanillaLinq(
            [FromQuery] OrderFilterDto filter)
        {
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
            return this.Ok(result);
        }

        [HttpGet("statuses")]
        [ProducesResponseType(typeof(List<KeyValuePairDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<KeyValuePairDto>>> GetOrderStatuses()
        {
            var orderStatuses = await this.dbContext.OrderStatuses
                .Select(os => new KeyValuePairDto { Key = os.Id, Value = os.Name }).ToListAsync();
            return this.Ok(orderStatuses);
        }
    }
}
