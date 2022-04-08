using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class UlongAdapter : ITypeAdapter
    {
        public Type UnderlyingType
        {
            get { return typeof(ulong); }
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
            ulong ul;
            bool result = ulong.TryParse(value, out ul);
            parsed = ul;
            return result;
        }
    }
}
