using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class LongAdapter : ITypeAdapter
    {
        public Type UnderlyingType
        {
            get { return typeof(long); }
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
            long l;
            bool result = long.TryParse(value, out l);
            parsed = l;
            return result;
        }
    }
}
