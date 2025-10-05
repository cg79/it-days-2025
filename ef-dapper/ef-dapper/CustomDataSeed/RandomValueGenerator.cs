namespace ef_dapper_CustomDataSeed;

public class RandomValueGenerator
{
    public static object GenerateRandomValue(Type targetType, double? min = null, double? max = null)
    {
        var rand = new Random();
        if (targetType == typeof(int) || targetType == typeof(long))
        {
            int minValue = (int)(min ?? 0);
            int maxValue = (int)(max ?? 1000);
            return rand.Next(minValue, maxValue);
        }
        else if (targetType == typeof(decimal) || targetType == typeof(double))
        {
            double minValue = min ?? 0;
            double maxValue = max ?? 1000;
            return minValue + rand.NextDouble() * (maxValue - minValue);
        }
        else if (targetType == typeof(string))
        {
            return $"Auto_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
        else if (targetType == typeof(DateTime))
        {
            return DateTime.Now.AddDays(-rand.Next(0, 365));
        }
        return null;
    }

}