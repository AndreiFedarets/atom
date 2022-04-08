using Atom.Design.Reflection.Metadata;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public sealed class TypeService : ITypeService
    {
        private readonly Dictionary<TypeReference, TypeAdapter> _adapters;
        private readonly Dictionary<string, ITypeExtension> _extensions;
        private readonly object _sync;

        public TypeService()
        {
            _sync = new object();
            _adapters = new Dictionary<TypeReference, TypeAdapter>();
            _extensions = new Dictionary<string, ITypeExtension>();
        }


        public TypeAdapter FindAdapter(TypeReference systemType)
        {
            lock (_sync)
            {
                TypeAdapter adapter;
                _adapters.TryGetValue(systemType, out adapter);
                return adapter;
            }
        }

        public bool RegisterExtension(ITypeExtension extension)
        {
            lock (_sync)
            {
                if (_extensions.ContainsKey(extension.DisplayName))
                {
                    return false;
                }
                IEnumerable<TypeAdapter> extensionAdapters = extension.Types;
                foreach (TypeAdapter extensionAdapter in extensionAdapters)
                {
                    if (_adapters.ContainsKey(extensionAdapter.SystemType))
                    {
                        return false;
                    }
                }
                foreach (TypeAdapter extensionAdapter in extensionAdapters)
                {
                    _adapters.Add(extensionAdapter.SystemType, extensionAdapter);
                }
                _extensions.Add(extension.DisplayName, extension);
                return true;
            }
        }

        public object ReadValue(TypeReference type, XElement valueElement)
        {
            lock (_sync)
            {
                TypeAdapter adapter = _adapters[type];
                return adapter.ReadValue(valueElement);
            }
        }

        public void WriteValue(TypeReference type, object value, XElement valueElement)
        {
            lock (_sync)
            {
                TypeAdapter adapter = _adapters[type];
                adapter.WriteValue(valueElement, value);
            }
        }

        public IEnumerable<CodeStatement> GenerateCode(TypeReference type, object value)
        {
            lock (_sync)
            {
                TypeAdapter adapter = _adapters[type];
                return adapter.GenerateCode(value);
            }
        }

        public IEnumerable<ITypeExtension> GetExtensions()
        {
            lock (_sync)
            {
                return new List<ITypeExtension>(_extensions.Values);
            }
        }
    }
}
