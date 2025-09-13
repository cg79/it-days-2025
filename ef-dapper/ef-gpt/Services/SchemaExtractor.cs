using System.Text;
using ef_base_repository;
using Microsoft.EntityFrameworkCore;
using TextToSqlDemo.Models;

namespace TextToSqlDemo.Services;

public static class SchemaExtractor
{
    public static string GetDatabaseSchema(DataContext context)
    {
        var schema = new StringBuilder();

        var entityTypes = context.Model.GetEntityTypes();
        foreach (var entity in entityTypes)
        {
            schema.AppendLine($"Tabela: {entity.GetTableName()}"); 

            foreach (var property in entity.GetProperties())
            {
                schema.AppendLine($" - {property.Name} ({property.ClrType.Name})");
            }

            schema.AppendLine();
        }

        return schema.ToString();
    }
}
