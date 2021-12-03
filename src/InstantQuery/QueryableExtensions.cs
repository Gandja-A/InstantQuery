using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using InstantQuery.Attributes;
using InstantQuery.ExpressionVisitors;
using InstantQuery.Interfaces;
using InstantQuery.Models;

namespace InstantQuery
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> FilterAndSort<T, TFilter>(this IQueryable<T> query, TFilter queryParams)
            where TFilter : ISortable
        {
            return query.Filter(queryParams).Sort(queryParams);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, object queryParams)
        {
            var expressionsMap = new Dictionary<CombineType, List<Expression<Func<T, bool>>>>
            {
                { CombineType.Or, new List<Expression<Func<T, bool>>>() },
                { CombineType.And, new List<Expression<Func<T, bool>>>() }
            };

            var queryConfigurations = GetQueryConfigurations(queryParams);

            foreach(var configuration in queryConfigurations)
            {
                switch(configuration.QueryType)
                {
                    case QueryType.Compare:
                        {
                            var lambda = Statements.Compare<T>(configuration.Value,
                                configuration.PropertyName,
                                configuration.CompareAs ?? ComparisonType.Equal,
                                configuration.ComparisonRestriction);
                            if(lambda != null)
                            {
                                expressionsMap[configuration.CombineAs].Add(lambda);
                            }
                        }
                        break;

                    case QueryType.Contains:
                        {
                            var lambda = Statements.ContainsIn<T>(configuration.Value,
                                configuration.PropertyType,
                                configuration.PropertyName);
                            if(lambda != null)
                            {
                                expressionsMap[configuration.CombineAs].Add(lambda);
                            }
                        }
                        break;
                    case QueryType.Search:
                        {
                            var lambda = Statements.Search<T>(configuration.Value,
                                configuration.PropertyName,
                                configuration.SearchType);
                            if(lambda != null)
                            {
                                expressionsMap[configuration.CombineAs].Add(lambda);
                            }
                        }
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            var exprComposeTypeAnd = expressionsMap[CombineType.And].Any()
                ? expressionsMap[CombineType.And].Aggregate((c, n) => c.Compose(n, CombineType.And))
                : null;

            var exprComposeTypeOr = expressionsMap[CombineType.Or].Any()
                ? expressionsMap[CombineType.Or].Aggregate((c, n) => c.Compose(n, CombineType.Or))
                : null;

            if(exprComposeTypeOr != null && exprComposeTypeAnd != null)
            {
                var finalLambda = exprComposeTypeOr.Compose(exprComposeTypeAnd, CombineType.And);
                query = query.Where(finalLambda);
            }
            else
            {
                var finalLambda = exprComposeTypeOr ?? exprComposeTypeAnd;
                if(finalLambda != null)
                {
                    query = query.Where(finalLambda);
                }
            }

            return query;
        }

        public static IQueryable<T> TakePage<T>(this IOrderedQueryable<T> query, IPaging paging)
        {
            if(paging.PageSize <= 0)
            {
                throw new ArgumentException("PageSize must be > 0");
            }

            if(paging.Page <= 0)
            {
                throw new ArgumentException("Page must be > 0");
            }

            return query.Skip((paging.Page - 1) * paging.PageSize).Take(paging.PageSize);
        }

        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> query, ISortable filters)
        {
            if(string.IsNullOrEmpty(filters.SortBy))
            {
                return query.OrderBy(_ => 0);
            }

            return query.OrderBy(filters.SortBy, filters.SortDir);
        }

        public static ListResult<T> ToListResult<T, TFilter>(this IQueryable<T> query, TFilter queryParams)
            where TFilter : IPaging, ISortable
        {
            var filteredAndSortedQuery = query.Filter(queryParams).Sort(queryParams);

            var totalCount = filteredAndSortedQuery.Count();

            var data = filteredAndSortedQuery.TakePage(queryParams).ToList();

            return new ListResult<T> { Data = data, TotalCount = totalCount };
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortBy, string sortDir)
        {
            if(string.IsNullOrEmpty(sortDir))
            {
                sortDir = "asc";
            }

            var sortByFields = sortBy.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var sortDirs = sortDir.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for(var i = 0; i < sortByFields.Length - sortDirs.Count; i++)
            {
                sortDirs.Add(sortDirs[0]);
            }

            var ordersPairs = sortByFields.Zip(sortDirs, (by, dir) => (by.Trim(), dir.Trim()));
            foreach(var sortsConf in ordersPairs)
            {
                var (sortByField, sortDirection) = sortsConf;
                query = query.ApplySort(sortByField, sortDirection);
            }

            return (IOrderedQueryable<T>)query;
        }

        private static IQueryable<T> ApplySort<T>(this IQueryable<T> queryable, string sortBy, string sortDir)
        {
            var isAscending = sortDir == "asc";
            var visitor = new OrderingMethodFinder();

            visitor.Visit(queryable.Expression);

            if(visitor.OrderingMethodFound)
            {
                queryable = isAscending
                    ? ((IOrderedQueryable<T>)queryable).ThenBy(ToLambda<T>(sortBy))
                    : ((IOrderedQueryable<T>)queryable).ThenByDescending(ToLambda<T>(sortBy));
            }
            else
            {
                queryable = isAscending
                    ? queryable.OrderBy(ToLambda<T>(sortBy))
                    : queryable.OrderByDescending(ToLambda<T>(sortBy));
            }

            return queryable;
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        private static IEnumerable<QueryConfiguration> GetQueryConfigurations(object queryParams)
        {
            var queryConfigurations = queryParams.GetType().GetProperties().Select(p =>
            {
                if(p.GetCustomAttributes(typeof(BaseQueryAttribute)) is not IEnumerable<BaseQueryAttribute>
                        attributes ||
                    !attributes.Any())
                {
                    return null;
                }

                var queryAttributes = attributes.OfType<QueryAttribute>().ToList();
                var restrictionAttributes = attributes.OfType<ComparisonRestrictionAttribute>().ToList();

                if(queryAttributes.Count > 1)
                {
                    throw new ArgumentException(
                        "More than one query attribute applies to the same field. Only one is allowed.");
                }

                if(!queryAttributes.Any() && restrictionAttributes.Any())
                {
                    throw new ArgumentException(
                        "The query attribute is missing. The restriction attribute uses without the query attribute. Add the query attribute.");
                }

                var queryAttr = queryAttributes.First();
                var restrictionAttr = restrictionAttributes.FirstOrDefault();

                return new QueryConfiguration
                {
                    Value = p.GetValue(queryParams),
                    PropertyType = p.PropertyType,
                    PropertyName = string.IsNullOrEmpty(queryAttr.For) ? p.Name : queryAttr.For,
                    CombineAs = queryAttr.CombineAs,
                    SearchType = (queryAttr as SearchByAttribute)?.SearchAs,
                    ComparisonRestriction =
                        restrictionAttr?.Restriction ?? ComparisonRestriction.None,
                    QueryType = queryAttr switch
                    {
                        CompareAttribute => QueryType.Compare,
                        ContainsAttribute => QueryType.Contains,
                        SearchByAttribute => QueryType.Search,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    CompareAs = queryAttr switch
                    {
                        CompareAttribute compareAttribute => compareAttribute.CompareAs,
                        _ => null
                    }
                };
            }).Where(q => q?.Value != null);
            return queryConfigurations;
        }
    }
}
