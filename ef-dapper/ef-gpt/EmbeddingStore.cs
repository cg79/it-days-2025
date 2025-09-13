namespace ef_gpt;

using Npgsql;
using System.Text.Json;
using Pgvector;

public class EmbeddingStore
{
    private readonly string _connectionString;

    public EmbeddingStore(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task EnsureTableAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
        CREATE EXTENSION IF NOT EXISTS vector;
        CREATE TABLE IF NOT EXISTS embeddings (
            id SERIAL PRIMARY KEY,
            reference_id TEXT NOT NULL,
            embedding vector(1536),
            metadata JSONB
        );";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task InsertEmbeddingAsync(string referenceId, float[] embedding, object? metadata = null)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var metadataJson = metadata != null ? JsonSerializer.Serialize(metadata) : "{}";

        await using var cmd = new NpgsqlCommand(@"
            INSERT INTO embeddings (reference_id, embedding, metadata)
            VALUES (@ref, @embedding, @meta)", conn);

        cmd.Parameters.AddWithValue("ref", referenceId);
        // cmd.Parameters.AddWithValue("embedding", embedding);
        cmd.Parameters.AddWithValue("embedding", new Vector(embedding));

        // cmd.Parameters.AddWithValue("meta", metadataJson);
        var metaParam = cmd.Parameters.AddWithValue("meta", NpgsqlTypes.NpgsqlDbType.Jsonb, metadataJson);


        await cmd.ExecuteNonQueryAsync();
    }



    public async Task<(string referenceId, float[] embedding, string metadata)[]> SearchSimilarAsync(float[] queryEmbedding, int topK = 5)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(@"
        SELECT reference_id, embedding, metadata
        FROM embeddings
        ORDER BY embedding <-> CAST(@query AS vector)
        LIMIT @k", conn);

        cmd.Parameters.AddWithValue("query", new Vector(queryEmbedding));
        cmd.Parameters.AddWithValue("k", topK);

        var results = new List<(string, float[], string)>();

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var refId = reader.GetString(0);
            var emb = ((Vector)reader["embedding"]).ToArray();  // properly map to float[]
            var meta = reader.GetString(2);
            results.Add((refId, emb, meta));
        }

        return results.ToArray();
    }


}
