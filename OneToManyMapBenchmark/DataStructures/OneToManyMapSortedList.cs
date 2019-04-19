using System;
using System.Collections.Generic;
using System.Text;

namespace OneToManyMapBenchmark
{
    internal sealed class OneToManyMapSortedList<TKey, TValue> : IOneToManyMap<TKey, TValue>
    {
        private static SortedList<TKey, int> messageToIdKvp = new SortedList<TKey, int>();
        private static SortedList<int, TKey> idToMessageKvp = new SortedList<int, TKey>();
        private static SortedList<TValue, int> stateToMessageIdKvp = new SortedList<TValue, int>();

        public TKey this[TValue value] => GetStateMessage(value);

        public void AddOneToManyMapping(TKey key, TValue[] values)
        {
            int messageId;
            if (!messageToIdKvp.TryGetValue(key, out messageId))
            {
                messageId = messageToIdKvp.Count + 1;
                messageToIdKvp[key] = messageId;
                idToMessageKvp[messageId] = key;
            }

            foreach (var state in values)
            {
                stateToMessageIdKvp[state] = messageId;
            }
        }

        private static TKey GetStateMessage(TValue value)
        {
            var messageId = stateToMessageIdKvp[value];
            return idToMessageKvp[messageId];
        }
    }
}
