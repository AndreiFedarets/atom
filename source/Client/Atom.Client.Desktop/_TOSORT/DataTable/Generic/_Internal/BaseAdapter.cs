using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal abstract class BaseAdapter : ITypeAdapter
    {
        protected BaseAdapter(Type type, object defaultValue)
        {
            UnderlyingType = type;
            DefaultValue = defaultValue;
        }

        public Type UnderlyingType { get; private set; }

        public string TypeName
        {
            get { return UnderlyingType.FullName; }
        }

        public object DefaultValue { get; private set; }

        public virtual string ToString(object value)
        {
            return value.ToString();
        }

        public abstract bool TryParse(string value, out object parsed);
    }
}
