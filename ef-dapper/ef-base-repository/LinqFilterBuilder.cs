namespace ef_base_repository;

using System.Linq.Expressions;
using System.Reflection;

public static class LinqFilterBuilder<T>
{
    public static Expression<Func<T, bool>> Build(FilterGroup group)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = BuildGroup(group, param);
        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    private static Expression BuildGroup(FilterGroup group, ParameterExpression param)
    {
        Expression result = null!;
        foreach (var filter in group.Filters)
        {
            Expression next;
            if (filter is FilterCondition cond)
            {
                next = BuildCondition(cond, param);
            }
            else if (filter is FilterGroup nestedGroup)
            {
                next = BuildGroup(nestedGroup, param);
            }
            else
            {
                throw new InvalidOperationException("Unsupported filter type");
            }

            if (result == null)
            {
                result = next;
            }
            else
            {
                result = group.Logic?.ToLower() == "or"
                    ? Expression.OrElse(result, next)
                    : Expression.AndAlso(result, next);
            }
        }
        return result;
    }

    private static Expression BuildCondition(FilterCondition cond, ParameterExpression param)
    {
        var member = Expression.PropertyOrField(param, cond.Field);

        switch (cond.Operator)
        {
            case FilterOperator.IsNull:
                return Expression.Equal(member, Expression.Constant(null, member.Type));

            case FilterOperator.IsNotNull:
                return Expression.NotEqual(member, Expression.Constant(null, member.Type));

            case FilterOperator.Contains:
            case FilterOperator.StartsWith:
            case FilterOperator.EndsWith:
                return BuildStringMethod(member, cond.Operator.ToString(), cond.Value);

            default:
                // Convert constant to the member type (handles nullable)
                var targetType = member.Type;
                var value = Convert.ChangeType(cond.Value, Nullable.GetUnderlyingType(targetType) ?? targetType);
                var constant = Expression.Constant(value, targetType);

                return cond.Operator switch
                {
                    FilterOperator.Eq  => Expression.Equal(member, constant),
                    FilterOperator.Neq => Expression.NotEqual(member, constant),
                    FilterOperator.Lt  => Expression.LessThan(member, constant),
                    FilterOperator.Lte => Expression.LessThanOrEqual(member, constant),
                    FilterOperator.Gt  => Expression.GreaterThan(member, constant),
                    FilterOperator.Gte => Expression.GreaterThanOrEqual(member, constant),
                    _ => throw new NotSupportedException($"Unsupported operator {cond.Operator}")
                };
        }
    }



    private static Expression BuildStringMethod(Expression member, string methodName, object value)
    {
        var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
        var constant = Expression.Constant(value, typeof(string));
        return Expression.Call(member, method!, constant);
    }
}
