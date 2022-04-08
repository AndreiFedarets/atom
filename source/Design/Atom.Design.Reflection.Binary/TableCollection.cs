using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design.Reflection.Binary
{
    [Serializable]
    public sealed class TableCollection : ReadOnlyDictionary<TypeReference, ITable>, ITableCollection
    {
        public TableCollection(Dictionary<TypeReference, ITable> collection)
            : base(collection)
        {
        }
    }
}
