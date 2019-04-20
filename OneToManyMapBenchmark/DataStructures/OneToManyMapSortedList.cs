using System;
using System.Collections.Generic;
using System.Text;

namespace OneToManyMapBenchmark
{
    internal sealed class OneToManyMapSortedList<TKey, TValue> : IOneToManyMap<TKey, TValue>
    {
        private SortedList<TKey, int> keyToKeyIdKvp = new SortedList<TKey, int>();
        private SortedList<int, TKey> keyIdToKeyKvp = new SortedList<int, TKey>();
        private SortedList<TValue, int> valueToKeyIdKvp = new SortedList<TValue, int>();

        public TKey this[TValue value] => GetKey(value);

        public void AddOneToManyMapping(TKey key, TValue[] values)
        {
            EnsureValuesHaveNoPriorMapping(values);

            int keyId;
            if (!keyToKeyIdKvp.TryGetValue(key, out keyId))
            {
                keyId = keyToKeyIdKvp.Count + 1;
                keyToKeyIdKvp[key] = keyId;
                keyIdToKeyKvp[keyId] = key;
            }

            foreach (var value in values)
            {
                valueToKeyIdKvp[value] = keyId;
            }
        }

        private void EnsureValuesHaveNoPriorMapping(TValue[] values)
        {
            foreach (var value in values)
            {
                if (valueToKeyIdKvp.ContainsKey(value))
                {
                    var keyId = valueToKeyIdKvp[value];
                     var mappedKey = keyIdToKeyKvp[keyId];
                    throw new ValuesHasPriorMappingToKeyException($"The value: {value}, has a prior mapping to the key: {mappedKey}.");
                }
            }
        }

        private TKey GetKey(TValue value)
        {
            if (valueToKeyIdKvp.ContainsKey(value))
            {
                var keyId = valueToKeyIdKvp[value];
                return keyIdToKeyKvp[keyId];
            }

            throw new ValueNotMappedToKeyException($"The value: {value} of type: {typeof(TValue)}, has not been mapped to a Key of type: {typeof(TKey)}");
        }
    }
}