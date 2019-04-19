using System;
using System.Collections.Generic;
using System.Text;

namespace OneToManyMapBenchmark
{
    internal interface IOneToManyMap<TKey, in TValue>
    {
        void AddOneToManyMapping(TKey key, TValue[] values);
        TKey this[TValue value] { get; }
    }
}
