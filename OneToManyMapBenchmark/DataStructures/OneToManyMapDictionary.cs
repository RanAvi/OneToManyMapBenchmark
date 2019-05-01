using System.Collections.Generic;

namespace OneToManyMapBenchmark
{
    internal class OneToManyMapDictionary<TKey, TValue> : IOneToManyMap<TKey, TValue>
    {
        private readonly Dictionary<TKey, int> keyToIdDict = new Dictionary<TKey, int>();
        private readonly Dictionary<int, TKey> idToKeyDict = new Dictionary<int, TKey>();
        private readonly Dictionary<TValue, int> valueToKeyIdDict = new Dictionary<TValue, int>();


        public TKey this[TValue value]
        {
            get
            {
                if (valueToKeyIdDict.TryGetValue(value, out int keyId))
                {
                    return idToKeyDict[keyId];
                }

                throw new ValueNotMappedToKeyException($"The value: {value} of type: {typeof(TValue)}, has not been mapped to a Key of type: {typeof(TKey)}");
            }
        }

        public void AddOneToManyMapping(TKey key, TValue[] values)
        {
            EnsureValuesHaveNoPriorMapping(values);

            if (!keyToIdDict.TryGetValue(key, out int keyId))
            {
                keyId = keyToIdDict.Count + 1;
                keyToIdDict.Add(key, keyId);
                idToKeyDict.Add(keyId, key);
            }

            foreach (var value in values)
            {
                valueToKeyIdDict.Add(value, keyId);
            }
        }

        private void EnsureValuesHaveNoPriorMapping(TValue[] values)
        {
            foreach (var value in values)
            {
                if (valueToKeyIdDict.ContainsKey(value))
                {
                    var keyId = valueToKeyIdDict[value];
                    var mappedKey = idToKeyDict[keyId];
                    throw new ValuesHasPriorMappingToKeyException($"The value: {value}, has a prior mapping to the key: {mappedKey}.");
                }
            }
        }
    }
}
