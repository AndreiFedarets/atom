using System;
using System.Collections.Generic;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    public static class TypeAdapterFactory
    {
        private static readonly Dictionary<Type, ITypeAdapter> _adapters;
        private static readonly UnsupportedAdapter _unsupportedAdapter;

        static TypeAdapterFactory()
        {
            _adapters = new Dictionary<Type, ITypeAdapter>();
            _adapters.Add(typeof(string), new StringAdapter());
            _adapters.Add(typeof(int), new IntAdapter());
            _unsupportedAdapter = new UnsupportedAdapter();
        }

        public static IEnumerable<ITypeAdapter> AvailableTypes
        {
            get { return _adapters.Values;  }
        }

        public static ITypeAdapter GetAdapter(string typeName)
        {
            Type type = Type.GetType(typeName);
            return GetAdapter(type);
        }

        public static ITypeAdapter GetAdapter(Type valueType)
        {
            ITypeAdapter adapter;
            if (_adapters.TryGetValue(valueType, out adapter))
            {
                return adapter;
            }
            //if (valueType.IsEnum)
            //{
            //    adapter = new EnumAdapter(valueType);
            //    _adapters.Add(valueType, adapter);
            //}
            //else
            //{
                adapter = _unsupportedAdapter;
            //}
            return adapter;
        }
    }
}
