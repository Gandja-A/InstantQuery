using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using InstantQuery.Attributes;

namespace InstantQuery
{
    internal static class Statements
    {
        private static readonly MethodInfo ToLower = typeof(string)
            .GetMethod("ToLower", new Type[] { });

        private static readonly MethodInfo Contains = typeof(string)
            .GetMethod("Contains", new[] { typeof(string) });

        private static readonly MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] { typeof(string) });

        public static Expression<Func<T, bool>> ContainsIn<T>(object propertyValue,
            Type propertyType, string propertyName)
        {
            var values = ((IList)propertyValue).Cast<object>().ToList();

            if(!values.Any())
            {
                return null;
            }

            var argumentType = values.First().GetType();

            var methodInfo = propertyType.GetMethod(nameof(Enumerable.Contains), new[] { argumentType });

            if(methodInfo is null)
            {
                throw new ArgumentException($"The type of property {propertyName} doesn't allow Contains method");
            }

            var value = Expression.Constant(propertyValue);

            var paramExp = Expression.Parameter(typeof(T), "x");

            var property = Expression.Property(paramExp, propertyName).ConvertToNullable();

            var body = Expression.Call(value, methodInfo, property);
            var lambda = Expression.Lambda<Func<T, bool>>(body, paramExp);
            return lambda;
        }

        public static Expression<Func<T, bool>> Compare<T>(object propertyValue, string propertyName,
            ComparisonType comparisonType, ComparisonRestriction comparisonRestriction)
        {
            var expressionType = (ExpressionType)Enum.Parse(typeof(ExpressionType), comparisonType.ToString(), true);

            var value = Expression.Constant(propertyValue);

            var paramExp = Expression.Parameter(typeof(T), "x");

            var property = Expression.Property(paramExp, propertyName).SetRestrictions(comparisonRestriction);

            var convertValue = Expression.Convert(value, property.Type);

            BinaryExpression binaryExpr;

            if(IfIsStringCanNotBeCompare(expressionType, property))
            {
                binaryExpr = StringComparisonExpression(propertyValue, property, expressionType);
            }
            else
            {
                binaryExpr = Expression.MakeBinary(expressionType, property, convertValue);
            }

            return Expression.Lambda<Func<T, bool>>(binaryExpr, paramExp);
        }

        public static Expression<Func<T, bool>> Search<T>(object propertyValue, string propertyName,
            SearchAs? searchType)
        {
            var value = Expression.Constant(propertyValue);

            var paramExp = Expression.Parameter(typeof(T), "x");

            var property = Expression.Property(paramExp, propertyName);

            var exp = Expression.Call(Expression.Call(property, ToLower),
                searchType == SearchAs.Contains
                    ? Contains
                    : StartsWith,
                value);
            return Expression.Lambda<Func<T, bool>>(exp, paramExp);
        }

        private static BinaryExpression StringComparisonExpression(object propertyValue, MemberExpression property,
            ExpressionType expressionType)
        {
            var methodInfo = typeof(string).GetMethod(nameof(string.CompareTo), new[] { typeof(string) });

            var methodCall = Expression.Call(property,
                methodInfo,
                Expression.Constant(propertyValue, typeof(string))
            );

            var binaryExpr = Expression.MakeBinary(expressionType,
                methodCall,
                Expression.Constant(0, typeof(int)));

            return binaryExpr;
        }

        private static bool IfIsStringCanNotBeCompare(ExpressionType expressionType, MemberExpression property)
        {
            var isNotSupportCompareOperation = expressionType is not ExpressionType.Equal &&
                                               expressionType is not ExpressionType.NotEqual;
            return property.Type == typeof(string) && isNotSupportCompareOperation;
        }

        private static MemberExpression SetRestrictions(this MemberExpression expressionType, ComparisonRestriction comparisonRestriction)
        {
            switch(comparisonRestriction)
            {
                case ComparisonRestriction.IgnoreTime:
                    if(expressionType.Type == typeof(DateTimeOffset))
                    {
                        expressionType = Expression.Property(expressionType, nameof(DateTimeOffset.Date));
                    }
                    if(expressionType.Type == typeof(DateTime))
                    {
                        expressionType = Expression.Property(expressionType, nameof(DateTime.Date));
                    }
                    break;
                default:
                    return expressionType;
            }
            return expressionType;
        }
    }
}
