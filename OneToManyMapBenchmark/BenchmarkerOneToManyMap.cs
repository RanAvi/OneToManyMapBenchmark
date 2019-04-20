using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneToManyMapBenchmark
{
    [MemoryDiagnoser]
    public class BenchmarkerOneToManyMap
    {
        private static OneToManyMapDataTable<string, string> oneToManyMapDataTable = new OneToManyMapDataTable<string, string>();
        private static OneToManyMapDictionary<string, string> messageToStateMap = new OneToManyMapDictionary<string, string>();
        private static OneToManyMapSortedList<string, string> oneToManyMapSortedList = new OneToManyMapSortedList<string, string>();

        private static string[] states = new string[] { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY" };

        [GlobalSetup]
        public void GlobalSetup()
        {
            Initialize(oneToManyMapDataTable);
            Initialize(messageToStateMap);
            Initialize(oneToManyMapSortedList);
        }

        private static void Initialize(IOneToManyMap<string, string> oneToManyMap)
        {
            oneToManyMap.AddOneToManyMapping("This is Message A", new[] { "VA", "MD", "SC" });
            oneToManyMap.AddOneToManyMapping("This is Message B", new[] { "CO", "WI", "NY" });
            oneToManyMap.AddOneToManyMapping("This is Message C", new[] { "LA", "MN", "FL" });
            oneToManyMap.AddOneToManyMapping("This is Message D", new[] { "IN", "IL" });
            oneToManyMap.AddOneToManyMapping("This is Message E", new[] { "AL", "AK" });
            oneToManyMap.AddOneToManyMapping("This is Message F", new[] { "AZ", "CA" });
            oneToManyMap.AddOneToManyMapping("This is Message G", new[] { "AR", "CT" });
            oneToManyMap.AddOneToManyMapping("This is Message H", new[] { "DE", "HI" });
            oneToManyMap.AddOneToManyMapping("This is Message I", new[] { "GA", "DC" });
            oneToManyMap.AddOneToManyMapping("This is Message J", new[] { "ID" });
            oneToManyMap.AddOneToManyMapping("This is Message K", new[] { "IA", "KS", "KY", "ME" });
            oneToManyMap.AddOneToManyMapping("This is Message L", new[] { "MA", "OK", "PA", "SD", "VT", "WY" });
            oneToManyMap.AddOneToManyMapping("This is Message M", new[] { "NE", "OH", "MI", "UT", "WA" });
            oneToManyMap.AddOneToManyMapping("This is Message N", new[] { "NV", "NH", "NJ", "NM", "NC", "ND", "OR", "RI", "TN", "TX", "WV" });
            oneToManyMap.AddOneToManyMapping("This is Message O", new[] { "MS", "MO", "MT" });
        }

        [Benchmark]
        public void TestOneToManyMapDataTable()
        {
            foreach (var state in states)
            {
                var message = oneToManyMapDataTable[state];
            }
        }

        [Benchmark(Baseline = true)]
        public void TestOneToManyMapSortedList()
        {
            foreach (var state in states)
            {
                var message = oneToManyMapSortedList[state];
            }
        }

        [Benchmark]
        public void TestOneToManyMapDictionary()
        {
            foreach (var state in states)
            {
                var message = messageToStateMap[state];
            }
        }
    }
}
