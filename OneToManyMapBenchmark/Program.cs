using BenchmarkDotNet.Running;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTestProject1")]

namespace OneToManyMapBenchmark
{
    static class Program
    {
        static void Main(string[] args)
        {
            ////var benchmarkerOneToManyMap = new BenchmarkerOneToManyMap();
            ////benchmarkerOneToManyMap.GlobalSetup();

            BenchmarkRunner.Run<BenchmarkerOneToManyMap>();
        }
    }
}
