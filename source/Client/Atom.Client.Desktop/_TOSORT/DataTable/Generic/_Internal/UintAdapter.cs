using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class UintAdapter : ITypeAdapter
    {
        public Type UnderlyingType
        {
            get { return typeof(uint); }
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
            uint ui;
            bool result = uint.TryParse(value, out ui);
            parsed = ui;
            return result;
        }
    }
}
