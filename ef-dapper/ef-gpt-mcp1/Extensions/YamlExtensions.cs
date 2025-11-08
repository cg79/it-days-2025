using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ef_gpt_mcp1.Extensions;

public static class YamlExtensions
{
    public static string ToYaml(this object obj)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        return serializer.Serialize(obj);
    }
}