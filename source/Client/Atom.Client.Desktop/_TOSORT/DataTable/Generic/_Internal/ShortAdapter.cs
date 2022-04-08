using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class ShortAdapter : ITypeAdapter
    {
        public Type UnderlyingType
        {
            get { return typeof(short); }
        }

        public string DefaultValue
        {
            get { return 0.ToString(); }
        }

        public string ToString(object value)
        {
            return value.ToString();
        }

        public bool TryParse(string value, out object parsed)
        {
            short s;
            bool result = short.TryParse(value, out s);
            parsed = s;
            return result;
        }
    }
}
