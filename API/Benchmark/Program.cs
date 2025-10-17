namespace Benchmark;

using Benchmark.Benchmarks;
using BenchmarkDotNet.Running;

internal static class Program
{
    static void Main(string[] args)
    {
        //Debug
        //var config = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
        //BenchmarkRunner.Run<ExcelExportBenchmark>(config);

        // Ensure running this in a Release build for accurate results
        BenchmarkRunner.Run<ExcelExportBenchmark>();
    }
}
