using System;
using System.Collections;
using System.Collections.Generic;

namespace Atom.Design
{
    public abstract class LazyReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Lazy<IDictionary<TKey, TValue>> _dictionary;

        public LazyReadOnlyDictionary()
        {
            _dictionary = new Lazy<IDictionary<TKey, TValue>>(Initialize);
        }

        public TValue this[TKey key]
        {
            get { return _dictionary.Value[key]; }
        }

        public IEnumerable<TKey> Keys
        {
            get { return _dictionary.Value.Keys; }
        }

        public IEnumerable<TValue> Values
        {
            get { return _dictionary.Value.Values; }
        }

        public int Count
        {
            get { return _dictionary.Value.Count; }
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.Value.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.Value.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.Value.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected abstract IDictionary<TKey, TValue> Initialize();
    }
}
