using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ef_base_repository;
public abstract class GenericExpression
{
    public static Expression<Func<T, bool>> CreateFilterExpression<T>(string filter)
    {
        return DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), false, filter);
    }

    public static IQueryable<T> ApplySorting<T>(IQueryable<T> source, Dictionary<string, string> sortCriteria)
    {
        if (sortCriteria == null || !sortCriteria.Any())
        {
            return source;
        }

        var orderByString = string.Join(", ",
            sortCriteria.Select(kvp => $"{kvp.Key} {kvp.Value}"));

        return source.OrderBy(orderByString);
    }

}
