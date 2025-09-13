
using Newtonsoft.Json;

namespace ef_base_repository;

public static class JsonExtensions
{
    public static string ToJson<T>(this T obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        return JsonConvert.SerializeObject(obj);
    }


    public static T FromJson<T>(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(json);
    }

}