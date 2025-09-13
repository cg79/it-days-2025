

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ef_implementation_benchmarks;

public static class BenchmarkHelper
{
    public static async Task RunBenchmark(
        Func<Task> action, 
        int executionsPerThread, 
        int threadCount = 10)
    {
        Console.WriteLine($"Starting benchmark: {threadCount} threads x {executionsPerThread} executions");

        var process = Process.GetCurrentProcess();
        var sw = Stopwatch.StartNew();

        // Capture baseline metrics
        long startMemory = GC.GetTotalMemory(forceFullCollection: true);
        TimeSpan startCpu = process.TotalProcessorTime;

        // Run the benchmark
        var tasks = new Task[threadCount];
        for (int i = 0; i < threadCount; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                for (int j = 0; j < executionsPerThread; j++)
                {
                    await action();
                }
            });
        }

        await Task.WhenAll(tasks);
        sw.Stop();

        // Capture end metrics
        long endMemory = GC.GetTotalMemory(forceFullCollection: true);
        TimeSpan endCpu = process.TotalProcessorTime;

        // Results
        var totalExecutions = threadCount * executionsPerThread;
        double avgExecTimeMs = sw.Elapsed.TotalMilliseconds / totalExecutions;

        Console.WriteLine("===== Benchmark Results =====");
        Console.WriteLine($"Threads: {threadCount}");
        Console.WriteLine($"Total Executions: {totalExecutions}");
        Console.WriteLine($"Total Time: {sw.Elapsed}");
        Console.WriteLine($"Avg Execution Time: {avgExecTimeMs:F2} ms");
        Console.WriteLine($"CPU Time: {(endCpu - startCpu).TotalMilliseconds} ms");
        Console.WriteLine($"Memory Delta: {(endMemory - startMemory) / 1024.0:F2} KB");
        Console.WriteLine("=============================");
    }
}
