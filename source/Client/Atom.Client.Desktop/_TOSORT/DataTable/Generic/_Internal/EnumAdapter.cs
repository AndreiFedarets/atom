using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class EnumAdapter : ITypeAdapter
    {
        private readonly Type _type;

        public EnumAdapter(Type type)
        {
            _type = type;
        }

        public Type UnderlyingType
        {
            get { return _type; }
        }

        public string DefaultValue
        {
            get
            {
                object[] values = (object[])Enum.GetValues(_type);
                return values[0].ToString();
            }
        }

        public string ToString(object value)
        {
            return value.ToString();
        }

        public bool TryParse(string value, out object parsed)
        {
            try
            {
                parsed = Enum.Parse(_type, value);
                return true;
            }
            catch (Exception)
            {
                parsed = null;
                return false;
            }
        }
    }
}
