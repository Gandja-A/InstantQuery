using System;
using System.Linq.Expressions;
using InstantQuery.Attributes;
using InstantQuery.ExpressionVisitors;

namespace InstantQuery
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> Compose<T>(
            this Expression<Func<T, bool>> first,
            LambdaExpression second, CombineType combineType)
        {
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = combineType == CombineType.Or
                ? Expression.OrElse(first.Body, second.Body)
                : Expression.AndAlso(first.Body, second.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
            return finalExpr;
        }

        public static MemberExpression ConvertToNullable(this MemberExpression membExp)
        {
            if(Utils.IsNullable(membExp.Type))
            {
                membExp = Expression.Property(membExp, nameof(Nullable<int>.Value));
            }

            return membExp;
        }
    }
}
