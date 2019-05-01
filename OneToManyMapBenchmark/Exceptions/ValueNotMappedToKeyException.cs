using System;

namespace OneToManyMapBenchmark
{
    [Serializable]
    public sealed class ValueNotMappedToKeyException : Exception
    {
        public ValueNotMappedToKeyException() { }
        public ValueNotMappedToKeyException(string message) : base(message) { }
        public ValueNotMappedToKeyException(string message, Exception inner) : base(message, inner) { }
        private ValueNotMappedToKeyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
