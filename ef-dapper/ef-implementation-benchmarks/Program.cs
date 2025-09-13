using BenchmarkDotNet.Running;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Exporters;
using ef_dapper_models;
using ef_implementation_benchmarks;

class Program
{
    
    
    static void Main(string[] args)
    {
        // BenchmarkRunner.Run<UserBenchmarksInsert>();
        // BenchmarkRunner.Run<UserBenchmarksUpdateByIdEF>();
        // BenchmarkRunner.Run<UserBenchmarksUpdateByIdDapper>();
        //
        BenchmarkRunner.Run<UserBenchmarksFind>();
        // BenchmarkRunner.Run<UserBenchmarksFindMT>();
    }
    
    // public static async Task Main(string[] args)
    // {
    //     var dapperMethods = new UserBenchmarksUpdateByIdDapper();
    //     dapperMethods.Setup();
    //     await BenchmarkHelper.RunBenchmark(
    //         async () =>
    //         {
    //             await dapperMethods.Dapper_UpdateByIdUsingEntityAndRawSql();
    //         },
    //         executionsPerThread: 10,  // how many times per thread
    //         threadCount: 10            // number of threads
    //     );
    // }
}