namespace OneToManyMapBenchmark
{
    internal interface IOneToManyMap<TKey, in TValue>
    {
        void AddOneToManyMapping(TKey key, TValue[] values);
        TKey this[TValue value] { get; }
    }
}
