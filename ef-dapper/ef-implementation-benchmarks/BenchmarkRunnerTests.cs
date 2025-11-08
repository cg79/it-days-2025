using Xunit;
using BenchmarkDotNet.Running;

public class BenchmarkRunnerTests
{
    [Fact(Skip = "Run manually")]
    public void RunBenchmarks()
    {
        BenchmarkRunner.Run<UserBenchmarks>();
    }
}