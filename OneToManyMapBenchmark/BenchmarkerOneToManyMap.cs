using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace OneToManyMapBenchmark
{
    [MemoryDiagnoser]
    public class BenchmarkerOneToManyMap
    {
        private static OneToManyMapDataTable<string, string> oneToManyMapDataTable = new OneToManyMapDataTable<string, string>();
        private static OneToManyMapDictionary<string, string> oneToManyMapDictionary = new OneToManyMapDictionary<string, string>();
        private static OneToManyMapSortedList<string, string> oneToManyMapSortedList = new OneToManyMapSortedList<string, string>();
        private static OneToManyMapList<string, string> oneToManyMapList = new OneToManyMapList<string, string>();

        ////private static string[] states = new string[] { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY" };
        private string[] randomizedValues;

        [Params(50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 2000, 3000)]
        public int CountOfValues { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Initialize();
        }

        private void Initialize()
        {
            randomizedValues = Randomizer.GetRandomUniqueAciiStrings(10, CountOfValues);
            var numOfValuesPerKey = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var tempDictionary = new Dictionary<string, string[]>();

            var indexPosition = 0;
            do
            {
                var statesInEntry = Randomizer.ShuffleArray(numOfValuesPerKey)[0];
                var statesMappedToKey = randomizedValues.Skip(indexPosition).Take(statesInEntry);
                indexPosition += statesInEntry;

                tempDictionary.Add("Key_" + Randomizer.GetRandomAciiString(10), statesMappedToKey.ToArray());

            } while (indexPosition < randomizedValues.Length);


            foreach (var key in tempDictionary.Keys)
            {
                var values = tempDictionary[key];
                oneToManyMapDataTable.AddOneToManyMapping(key, values);                
                oneToManyMapList.AddOneToManyMapping(key, values);
                oneToManyMapSortedList.AddOneToManyMapping(key, values);
                oneToManyMapDictionary.AddOneToManyMapping(key, values);
            }
        }

        [Benchmark]
        public void TestOneToManyMapDataTable()
        {
            foreach (var value in randomizedValues)
            {
                var _ = oneToManyMapDataTable[value];
            }
        }

        [Benchmark]
        public void TestOneToManyMapSortedList()
        {
            foreach (var value in randomizedValues)
            {
                var _ = oneToManyMapSortedList[value];
            }
        }

        [Benchmark]
        public void TestOneToManyMapList()
        {
            foreach (var value in randomizedValues)
            {
                var _ = oneToManyMapList[value];
            }
        }

        [Benchmark(Baseline = true)]
        public void TestOneToManyMapDictionary()
        {
            foreach (var value in randomizedValues)
            {
                var _ = oneToManyMapDictionary[value];
            }
        }
    }
}
