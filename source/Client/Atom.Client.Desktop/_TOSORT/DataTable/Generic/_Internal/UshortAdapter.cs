using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class UshortAdapter : ITypeAdapter
    {
        public Type UnderlyingType
        {
            get { return typeof(ushort); }
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
            ushort us;
            bool result = ushort.TryParse(value, out us);
            parsed = us;
            return result;
        }
    }
}
