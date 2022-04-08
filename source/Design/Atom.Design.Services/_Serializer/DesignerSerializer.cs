using Atom.Design.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Services
{
    public sealed class DesignerSerializer : IDesignerSerializer
    {
        private readonly Dictionary<Type, IDesignerSerializer> _serializers;

        public DesignerSerializer()
        {
            _serializers = new Dictionary<Type, IDesignerSerializer>();
        }

        public void AddSerializer<T>(IDesignerSerializer serializer)
        {
            _serializers.Add(typeof(T), serializer);
        }

        public bool CanRead(IDocument document)
        {
            return _serializers.Values.Any(x => x.CanRead(document));
        }

        public IObjectDesigner Read(IDocument document)
        {
            IObjectDesigner designer = null;
            foreach (IDesignerSerializer serializer in _serializers.Values)
            {
                if (serializer.CanRead(document))
                {
                    designer = serializer.Read(document);
                    designer.Document = document;
                    break;
                }
            }
            return designer;
        }

        public bool Write(IObjectDesigner designer)
        {
            IDesignerSerializer serializer;
            if (!_serializers.TryGetValue(designer.GetType(), out serializer))
            {
                return false;
            }
            return serializer.Write(designer);
        }
    }
}
