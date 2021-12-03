using System.Linq.Expressions;

namespace InstantQuery.ExpressionVisitors
{
    internal class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression parameter;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(this.parameter);
        }

        internal ParameterReplacer(ParameterExpression parameter)
        {
            this.parameter = parameter;
        }
    }
}
