using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTestProject1")]

namespace OneToManyMapBenchmark
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
