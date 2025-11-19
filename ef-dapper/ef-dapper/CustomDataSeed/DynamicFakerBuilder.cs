

using Bogus;
using Newtonsoft.Json.Linq;

namespace ef_dapper_CustomDataSeed;

public static class DynamicFakerBuilder
{
    public static Faker<Dictionary<string, object>> Build(JObject rulesJson)
    {
        var faker = new Faker<Dictionary<string, object>>()
            .CustomInstantiator(_ => new Dictionary<string, object>());

        foreach (var rule in rulesJson)
        {
            string key = rule.Key;
            var def = (JObject)rule.Value;

            faker.RuleFor(dict => dict[key], (f, dict) =>
            {
                string type = def["type"]?.ToString();

                switch (type)
                {
                    case "dateBetween":
                        return f.Date.Between(
                            DateTime.Parse(def["start"]!.ToString()),
                            DateTime.Parse(def["end"]!.ToString())
                        );

                    case "userName":
                        return f.Internet.UserName();

                    case "fullName":
                        return f.Name.FullName();

                    case "long":
                        long min = def["min"]?.ToObject<long>() ?? 0;
                        long max = def["max"]?.ToObject<long>() ?? long.MaxValue;
                        return f.Random.Long(min, max);

                    case "string":
                        return f.Random.AlphaNumeric(def["length"]?.ToObject<int>() ?? 10);

                    default:
                        return null!;
                }
            });
        }

        return faker;
    }
}
