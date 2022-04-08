using System;
using System.Diagnostics;

namespace Interop.Native.Accessible
{
    public class AccessiblePropertyBase<T>
    {
        private readonly T _defaultValue;
        private readonly Func<T> _valueExtractor;
        private readonly bool _cache;
        private T _cachedValue;
        private bool _isCached;

        public AccessiblePropertyBase(Func<T> valueExtractor, bool cache, T defaultValue)
        {
            _valueExtractor = valueExtractor;
            _cache = cache;
            _defaultValue = defaultValue;
        }

        public T Value
        {
            get
            {
                if (!_isCached || !_cache)
                {
                    try
                    {
                        _cachedValue = _valueExtractor();
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }
                    finally
                    {
                        if (_cachedValue == null || _cachedValue.Equals(default(T)))
                        {
                            _cachedValue = _defaultValue;
                        }
                        _isCached = true;
                    }
                }
                return _cachedValue;
            }
        }

        public void ResetCache()
        {
            _isCached = false;
        }
    }
}
