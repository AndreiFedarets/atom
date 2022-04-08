using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    public interface ITypeAdapter
    {
        string TypeName { get; }

        Type UnderlyingType { get; }

        object DefaultValue { get; }

        bool TryParse(string value, out object parsed);

        string ToString(object value);
    }
}
