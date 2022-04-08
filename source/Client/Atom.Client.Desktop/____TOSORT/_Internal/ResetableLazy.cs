using System;

namespace Atom
{
    internal sealed class ResetableLazy<T> : Lazy<T>
    {
        private readonly object _lock;
        private readonly Func<T> _activator;
        private volatile bool _valueCreated;
        private T _value;

        public ResetableLazy(Func<T> activator)
        {
            _lock = new object();
            _activator = activator;
        }

        public new T Value
        {
            get
            {
                if (!_valueCreated)
                {
                    lock (_lock)
                    {
                        if (!_valueCreated)
                        {
                            _value = _activator();
                            _valueCreated = true;
                        }
                    }
                }
                return _value;
            }
        }

        public void Reset()
        {
            _valueCreated = false;
        }
    }
}
