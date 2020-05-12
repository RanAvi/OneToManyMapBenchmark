using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OneToManyMapBenchmark
{
    internal class OneToManyMapList<TKey, TValue> : IOneToManyMap<TKey, TValue>
    {
        private readonly List<KeyId<TKey>> keyIdList = new List<KeyId<TKey>>();
        private readonly List<ValueKeyId<TValue>> valueKeyIdList = new List<ValueKeyId<TValue>>();

        public TKey this[TValue value]
        {
            get
            {
                var matchedVaueKeyId = FindValueKeyIdUsingValue(valueKeyIdList, value);
                if (matchedVaueKeyId != null)
                {
                    return FindKeyIdUsingId(keyIdList, matchedVaueKeyId.KeyId).Key;
                }

                throw new ValueNotMappedToKeyException($"The value: {value} of type: string, has not been mapped to a Key of type: string");
            }
        }

        public void AddOneToManyMapping(TKey key, TValue[] values)
        {
            EnsureValuesHaveNoPriorMapping(values);

            var matchedKeyId = FindKeyIdUsingKey(keyIdList, key);

            int keyId;
            if (matchedKeyId == null)
            {
                keyId = keyIdList.Count + 1;
                keyIdList.Add(new KeyId<TKey>(key, keyId));
            }
            else
            {
                keyId = matchedKeyId.Id;
            }

            foreach (var value in values)
            {
                valueKeyIdList.Add(new ValueKeyId<TValue>(value, keyId));
            }
        }

        private static KeyId<TKey> FindKeyIdUsingKey(List<KeyId<TKey>> list, TKey key)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item.Key.Equals(key))
                {
                    return item;
                }
            }

            return null;
        }

        private static KeyId<TKey> FindKeyIdUsingId(List<KeyId<TKey>> list, int id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item.Id == id)
                {
                    return item;
                }
            }

            return null;
        }

        private static ValueKeyId<TValue> FindValueKeyIdUsingValue(List<ValueKeyId<TValue>> list, TValue value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item.Value.Equals(value))
                {
                    return item;
                }
            }

            return null;
        }

        private void EnsureValuesHaveNoPriorMapping(TValue[] values)
        {
            foreach (var value in values)
            {
                var matchedValueKeyId = valueKeyIdList.Find(vki => vki.Value.Equals(value));
                if (matchedValueKeyId != null)
                {
                    var keyId = matchedValueKeyId.KeyId;
                    var mappedKey = keyIdList.Find(kId => kId.Id == keyId);
                    throw new ValuesHasPriorMappingToKeyException($"The value: {value}, has a prior mapping to the key: {mappedKey.Key}.");
                }
            }
        }
    }

    internal sealed class KeyId<TKey>
    {
        public int Id { get; }
        public TKey Key { get; }

        public KeyId(TKey key, int id)
        {
            Key = key;
            Id = id;
        }
    }

    internal sealed class ValueKeyId<TValue> : IEquatable<ValueKeyId<TValue>>
    {
        public int KeyId { get; }
        public TValue Value { get; }

        public ValueKeyId(TValue value, int keyId)
        {
            Value = value;
            KeyId = keyId;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as ValueKeyId<TValue>);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(ValueKeyId<TValue> other)
        {
            return other != null &&
                   KeyId == other.KeyId &&
                   EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return HashCode.Combine(KeyId, Value);
        }

        public static bool operator ==(ValueKeyId<TValue> left, ValueKeyId<TValue> right)
        {
            return EqualityComparer<ValueKeyId<TValue>>.Default.Equals(left, right);
        }

        public static bool operator !=(ValueKeyId<TValue> left, ValueKeyId<TValue> right)
        {
            return !(left == right);
        }
    }
}
