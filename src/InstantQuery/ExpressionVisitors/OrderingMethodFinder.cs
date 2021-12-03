using System;
using System.Linq;
using System.Linq.Expressions;

namespace InstantQuery.ExpressionVisitors
{
    internal class OrderingMethodFinder : ExpressionVisitor
    {
        public bool OrderingMethodFound { get; set; }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var name = node.Method.Name;

            if(node.Method.DeclaringType == typeof(Queryable) && (
                name.StartsWith("OrderBy", StringComparison.Ordinal) ||
                name.StartsWith("ThenBy", StringComparison.Ordinal)))
            {
                this.OrderingMethodFound = true;
            }

            return base.VisitMethodCall(node);
        }
    }
}
