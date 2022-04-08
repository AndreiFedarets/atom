using System;
using System.Collections;
using System.Collections.Generic;

namespace Atom.Design.Extension.Desktop
{
    public abstract class LazyReadOnlyCollection<T> : IReadOnlyList<T>
    {
        private readonly Lazy<IList<T>> _collection;

        public LazyReadOnlyCollection()
        {
            _collection = new Lazy<IList<T>>(Initialize);
        }

        public T this[int index]
        {
            get { return _collection.Value[index]; }
        }

        public int Count
        {
            get { return _collection.Value.Count; }
        }

        protected IList<T> Items
        {
            get { return _collection.Value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected abstract IList<T> Initialize();
    }
}
