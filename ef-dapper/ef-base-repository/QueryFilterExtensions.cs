using Dapper;

namespace ef_base_repository;

using System.Text;
using System.Linq;
using Dapper;
using System.Linq.Expressions;

public static class QueryFilterExtensions
{
     public static (string Sql, DynamicParameters Params) ToSqlWithParams(this QueryFilter filter)
    {
        var parameters = new DynamicParameters();

        if (filter?.Conditions == null || filter.Conditions.Count == 0)
            return (string.Empty, parameters);

        var parts = filter.Conditions.Select((c, i) =>
        {
            // Unique parameter name
            var paramName = $"{c.Field}{i}";

            // Handle string pattern operators
            object? value = c.Value;
            string sqlPart;

            switch (c.Operator)
            {
                case SqlOperator.Equals:
                    sqlPart = $"{c.Field} = @{paramName}";
                    break;

                case SqlOperator.NotEquals:
                    sqlPart = $"{c.Field} <> @{paramName}";
                    break;

                case SqlOperator.GreaterThan:
                    sqlPart = $"{c.Field} > @{paramName}";
                    break;

                case SqlOperator.GreaterOrEqual:
                    sqlPart = $"{c.Field} >= @{paramName}";
                    break;

                case SqlOperator.LessThan:
                    sqlPart = $"{c.Field} < @{paramName}";
                    break;

                case SqlOperator.LessOrEqual:
                    sqlPart = $"{c.Field} <= @{paramName}";
                    break;

                case SqlOperator.StartsWith:
                    sqlPart = $"{c.Field} LIKE @{paramName}";
                    value = $"{c.Value}%";
                    break;

                case SqlOperator.EndsWith:
                    sqlPart = $"{c.Field} LIKE @{paramName}";
                    value = $"%{c.Value}";
                    break;

                case SqlOperator.Contains:
                    sqlPart = $"{c.Field} LIKE @{paramName}";
                    value = $"%{c.Value}%";
                    break;

                case SqlOperator.In:
                    if (c.Value is not System.Collections.IEnumerable enumerable)
                        throw new ArgumentException("IN operator requires a list or array");

                    // Dapper will expand lists automatically if you use e.g. "WHERE Id IN @Ids"
                    sqlPart = $"{c.Field} IN @{paramName}";
                    break;

                default:
                    throw new NotSupportedException($"Operator {c.Operator} not supported");
            }

            // Add parameter for Dapper
            parameters.Add(paramName, value);

            return sqlPart;
        });

        var sql = string.Join(" AND ", parts);
        return (sql, parameters);
    }
    
    public static Expression<Func<T, bool>> ToExpression<T>(this QueryFilter filter)
    {
        if (filter?.Conditions == null || filter.Conditions.Count == 0)
            return x => true; // no conditions → always true

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? body = null;

        foreach (var condition in filter.Conditions)
        {
            var member = Expression.PropertyOrField(parameter, condition.Field);
            var memberType = member.Type;

            // Convert value to the underlying type if nullable
            object? typedValue;
            if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typedValue = Convert.ChangeType(condition.Value, Nullable.GetUnderlyingType(memberType)!);
            }
            else
            {
                typedValue = Convert.ChangeType(condition.Value, memberType);
            }

            var constant = Expression.Constant(typedValue, memberType);

            Expression comparison = condition.Operator switch
            {
                SqlOperator.Equals         => Expression.Equal(member, constant),
                SqlOperator.NotEquals      => Expression.NotEqual(member, constant),
                SqlOperator.GreaterThan    => Expression.GreaterThan(member, constant),
                SqlOperator.GreaterOrEqual => Expression.GreaterThanOrEqual(member, constant),
                SqlOperator.LessThan       => Expression.LessThan(member, constant),
                SqlOperator.LessOrEqual    => Expression.LessThanOrEqual(member, constant),
                SqlOperator.StartsWith     => BuildStringMethodCall(member, constant, "StartsWith"),
                SqlOperator.EndsWith       => BuildStringMethodCall(member, constant, "EndsWith"),
                SqlOperator.Contains       => BuildStringMethodCall(member, constant, "Contains"),
                _ => throw new NotSupportedException($"Operator {condition.Operator} not supported")
            };


            body = body == null ? comparison : Expression.AndAlso(body, comparison);
        }

        return Expression.Lambda<Func<T, bool>>(body!, parameter);
    }
    
    private static Expression BuildStringMethodCall(MemberExpression member, ConstantExpression constant, string methodName)
    {
        if (member.Type != typeof(string))
            throw new InvalidOperationException($"{methodName} operator can only be applied to string properties.");

        var method = typeof(string).GetMethod(methodName, new[] { typeof(string) })!;
        return Expression.Call(member, method, constant);
    }

    private static Expression BuildLikeExpression(MemberExpression member, ConstantExpression constant)
    {
        if (member.Type != typeof(string))
            throw new InvalidOperationException("LIKE operator can only be applied to string properties.");

        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
        return Expression.Call(member, method, constant);
    }
}

