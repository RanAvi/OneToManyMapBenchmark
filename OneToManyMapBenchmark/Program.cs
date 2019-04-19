using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTestProject1")]

namespace OneToManyMapBenchmark
{
    static class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkerOneToManyMap>();
            Console.WriteLine("Hello World!");
        }
    }
}
