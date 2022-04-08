using System.Collections;

namespace Interop.Native.Accessible
{
    public class AccessibleProperties : DictionaryBase
    {
        public void AddProperty<T>(string key, AccessiblePropertyBase<T> accessibleProperty)
        {
            Dictionary.Add(key, accessibleProperty);
        }

        public AccessiblePropertyBase<T> GetProperty<T>(string key)
        {
            return (AccessiblePropertyBase<T>)Dictionary[key];
        }

        public T GetPropertyValue<T>(string key)
        {
            AccessiblePropertyBase<T> accessibleProperty = GetProperty<T>(key);
            return accessibleProperty.Value;
        }

        public void ResetCache()
        {
            foreach (object item in Dictionary.Values)
            {
                if (item is StringProperty)
                {
                    (item as StringProperty).ResetCache();
                }
            }
        }
    }
}
