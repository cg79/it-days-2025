using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ef.Cryptography;

public class CryptographyHelper
{
    public static string GenerateCacheKeyFromJson(object obj)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string normalizedJson = JsonSerializer.Serialize(obj, options);
        return GenerateCacheKeyFromString(normalizedJson);
    }
    
    public static string GenerateCacheKeyFromString(string inputKey)
    {
        using var sha = SHA256.Create();
        byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(inputKey));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }
}