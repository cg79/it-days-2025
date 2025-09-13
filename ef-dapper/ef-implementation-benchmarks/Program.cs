using BenchmarkDotNet.Running;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Exporters;

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<UserBenchmarks>();
        // System.IO.File.WriteAllText("benchmark.html", BenchmarkDotNet.Reports.MarkdownExporter.Default.ExportToFile(summary));
    }
}