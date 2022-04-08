using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atom.Design.Reflection.Code
{
    public sealed class Table : ReadOnlyDictionary<string, ITableValue>, ITable, IEquatable<Table>
    {
        public Table(string title, TypeReference type, Dictionary<string, ITableValue> collection)
            : base(collection)
        {
            Title = title;
            Reference = type;
        }

        public string Title { get; private set; }

        public TypeReference Reference { get; private set; }

        public IEnumerable<IValueConsumer> Consumers
        {
            get { return Enumerable.Empty<IValueConsumer>(); }
        }

        public IEnumerable<IValueSource> Sources
        {
            get { return Values; }
        }

        public bool Equals(Table other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Reference.Equals(Reference);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Table);
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }
    }
}
