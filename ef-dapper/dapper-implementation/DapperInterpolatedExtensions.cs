using System.Data;
using Dapper;

namespace dapper_implementation;

public static class DapperInterpolatedExtensions
{
    public static Task<int> ExecuteInterpolatedAsync(
        this IDbConnection connection,
        FormattableString sql,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var (query, parameters) = sql.ToDapperParameters();
        return connection.ExecuteAsync(query, parameters, transaction);
    }

    public static Task<T> ExecuteScalarInterpolatedAsync<T>(
        this IDbConnection connection,
        FormattableString sql,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var (query, parameters) = sql.ToDapperParameters();
        return connection.ExecuteScalarAsync<T>(query, parameters, transaction);
    }

    private static (string, DynamicParameters) ToDapperParameters(this FormattableString sql)
    {
        var args = sql.GetArguments();
        var parameters = new DynamicParameters();
        var query = sql.Format;

        for (int i = 0; i < args.Length; i++)
        {
            var name = $"@p{i}";
            query = query.Replace("{" + i + "}", name);
            parameters.Add(name, args[i]);
        }

        return (query, parameters);
    }
}
