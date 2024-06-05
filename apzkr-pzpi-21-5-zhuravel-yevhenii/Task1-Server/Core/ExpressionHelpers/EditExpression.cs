using System.Linq.Expressions;
using System.Reflection;

namespace Core.ExpressionHelpers
{
    public static class EditExpression<T>
    {
        public static Expression<Func<T, bool>>? RemoveAttributedProperties<TAttribute>(Expression<Func<T, bool>> predicate)
        where TAttribute : Attribute
        {
            switch (predicate.Body.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    var left = RemoveAttributedProperties<TAttribute>(
                        Expression.Lambda<Func<T, bool>>(((BinaryExpression)predicate.Body).Left, predicate.Parameters));
                    var right = RemoveAttributedProperties<TAttribute>(
                        Expression.Lambda<Func<T, bool>>(((BinaryExpression)predicate.Body).Right, predicate.Parameters));
                    if (left is null && right is null)
                    {
                        return null;
                    }
                    if (left is null)
                    {
                        return right;
                    }
                    if (right is null)
                    {
                        return left;
                    }
                    else
                    {
                        return Expression.Lambda<Func<T, bool>>(Expression.MakeBinary(predicate.Body.NodeType, left.Body, right.Body), predicate.Parameters);
                    }
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThan:
                    return DeleteNode<TAttribute>((BinaryExpression)predicate.Body) ? null : predicate;
            }

            return predicate;
        }

        public static Expression<Func<T, bool>> ConcatExpressionsWithAnd(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, bool takeParametersFromLeft = true)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left.Body, right.Body),
                takeParametersFromLeft ? left.Parameters : right.Parameters
                );
        }

        public static Expression<Func<T, bool>> NegatePredicate(Expression<Func<T, bool>> predicate)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(predicate.Body), predicate.Parameters);
        }

        private static bool DeleteNode<TAttribute>(BinaryExpression binaryExpression)
            where TAttribute : Attribute
        {
            var left = binaryExpression.Left;
            var right = binaryExpression.Right;

            if (CheckNode<TAttribute>(left))
            {
                return true;
            }

            return CheckNode<TAttribute>(right);
        }

        private static bool CheckNode<TAttribute>(Expression node)
            where TAttribute : Attribute
        {
            switch (node.NodeType)
            {
                case ExpressionType.Convert:
                    if (((MemberExpression)((UnaryExpression)node).Operand).Member.GetCustomAttribute<TAttribute>() is not null)
                    {
                        return true;
                    }
                    break;
                case ExpressionType.MemberAccess:
                    if (((MemberExpression)node).Member.GetCustomAttribute<TAttribute>() is not null)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}
