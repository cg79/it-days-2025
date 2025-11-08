using ef_base_repository;
using Efcom.Base.Repository.Request;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace dapper_dommel_implementation;

using Dapper;
using Dommel;
using System.Text;
using System.Data;

public static class DommelPaginationExtensions
{
    public static async Task<PaginationResponse<T>> PaginateAsync<T>(
        this IDbConnection connection,
        PaginationRequest request,
        string? baseFilter = null) where T : class
    {
        var tableName = typeof(T).Name; 
        var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
        if (tableAttribute != null)
        {
            tableName = tableAttribute.Name;
        }
        var sql = new StringBuilder($"SELECT * FROM {tableName}");
        var countSql = new StringBuilder($"SELECT COUNT(*) FROM {tableName}");

        var whereClause = BuildWhereClause(request, baseFilter);
        if (!string.IsNullOrEmpty(whereClause))
        {
            sql.Append(" WHERE ").Append(whereClause);
            countSql.Append(" WHERE ").Append(whereClause);
        }

        var orderClause = BuildOrderBy(request);
        if (!string.IsNullOrEmpty(orderClause))
        {
            sql.Append(" ORDER BY ").Append(orderClause);
        }
        else
        {
            sql.Append(" ORDER BY (SELECT NULL)"); // avoid SQL error
        }

        int offset = (request.PageNo - 1) * request.PageSize;
        // sql.Append($" OFFSET {offset} ROWS FETCH NEXT {request.PageSize} ROWS ONLY");
        sql.Append($" LIMIT {request.PageSize} OFFSET {offset}");

        // Execute queries
        var query = sql.ToString();
        var data = await connection.QueryAsync<T>(query);
        var total = await connection.ExecuteScalarAsync<int>(countSql.ToString());

        return new PaginationResponse<T>
        {
            Items = data.ToList(),
            TotalCount = total,
            // PageNo = request.PageNo,
            // PageSize = request.PageSize
        };
    }

    private static string BuildOrderBy(PaginationRequest request)
    {
        if (request.SortCriteria == null || !request.SortCriteria.Any())
            return string.Empty;

        return string.Join(", ",
            request.SortCriteria.Select(s => $"{s.Key} {(s.Value ? "ASC" : "DESC")}"));
    }

    private static string BuildWhereClause(PaginationRequest request, string? baseFilter)
    {
        var clauses = new List<string>();

        if (!string.IsNullOrEmpty(baseFilter))
            clauses.Add(baseFilter);

        if (!string.IsNullOrEmpty(request.FilterExpression))
            clauses.Add(request.FilterExpression);

        if (request.FilterGroup != null)
        {
            var str = SqlFilterBuilder.BuildWhereClause(request.FilterGroup);
            clauses.Add(str);
        }

        return string.Join(" AND ", clauses);
    }

    // private static string BuildFilterGroupClause(FilterGroup group)
    // {
    //     var clauses = new List<string>();
    //
    //     if (!string.IsNullOrEmpty(group.Field) && !string.IsNullOrEmpty(group.Operator))
    //     {
    //         var valueStr = group.Operator.ToUpper().Contains("LIKE")
    //             ? $"'%{group.Value}%'"
    //             : $"'{group.Value}'";
    //
    //         clauses.Add($"[{group.Field}] {group.Operator} {valueStr}");
    //     }
    //
    //     if (group.Children != null && group.Children.Any())
    //     {
    //         var childClauses = group.Children.Select(BuildFilterGroupClause);
    //         var logicalOp = string.IsNullOrWhiteSpace(group.LogicalOperator) ? "AND" : group.LogicalOperator;
    //         clauses.Add($"({string.Join($" {logicalOp} ", childClauses)})");
    //     }
    //
    //     return string.Join(" AND ", clauses);
    // }
}
