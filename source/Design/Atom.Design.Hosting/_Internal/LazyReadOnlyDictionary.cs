using System.Collections;
using System.Collections.Generic;

namespace Atom.Design.Hosting
{
    internal abstract class LazyReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        protected readonly object Sync;
        private bool _initialized;
        private IDictionary<TKey, TValue> _dictionary;

        public LazyReadOnlyDictionary()
        {
            Sync = new object();
        }

        protected bool Initialized
        {
            get { return _initialized; }
        }

        protected IDictionary<TKey, TValue> Dictionary
        {
            get 
            {
                if (!_initialized)
                {
                    lock (Sync)
                    {
                        if (!_initialized)
                        {
                            _dictionary = InitializeDictionary();
                            _initialized = true;
                        }
                    }
                }
                return _dictionary;
            }
        }

        public TValue this[TKey key]
        {
            get 
            {
                lock (Sync)
                {
                    return Dictionary[key];
                }
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                lock (Sync)
                {
                    return Dictionary.Keys; 
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                lock (Sync)
                {
                    return Dictionary.Values;
                }
            }
        }

        public int Count
        {
            get 
            {
                lock (Sync)
                {
                    return Dictionary.Count;
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (Sync)
            {
                return Dictionary.ContainsKey(key);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (Sync)
            {
                return Dictionary.GetEnumerator();
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (Sync)
            {
                return Dictionary.TryGetValue(key, out value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (Sync)
            {
                return Dictionary.GetEnumerator();
            }
        }

        protected abstract IDictionary<TKey, TValue> InitializeDictionary();
    }
}
