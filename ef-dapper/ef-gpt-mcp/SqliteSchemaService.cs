using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using SqliteMCP.Models;

namespace SqliteMCP;

public class SqliteSchemaService(string connectionString)
{
    public List<SqliteTable> GetTables()
    {
        using var connection = new MySqlConnection(connectionString);

        var tables = connection.Query<SqliteTable>(
            """
            SELECT name AS Name, type AS Type 
            FROM sqlite_master 
            WHERE type = 'table' AND name NOT LIKE 'sqlite_%'
            ORDER BY name;
            """).ToList();

        return tables;
    }
    
    public string CreateTable(string tableName, Dictionary<string, string> columns)
    {
        if (columns.Count == 0)
            return $"Cannot create table '{tableName}' with no columns.";

        try
        {
            var columnsDef = string.Join(", ", columns.Select(kv => $"{QuoteIdentifier(kv.Key)} {kv.Value}"));
            var sql = $"CREATE TABLE {QuoteIdentifier(tableName)} ({columnsDef});";

            using var connection = new MySqlConnection(connectionString);
            connection.Execute(sql);

            return $"Successfully created table '{tableName}'.";
        }
        catch (Exception ex)
        {
            return $"Error creating table '{tableName}': {ex.Message}";
        }
    }
    
    private string QuoteIdentifier(string identifier) =>
        "\"" + identifier.Replace("\"", "\"\"") + "\"";

}
