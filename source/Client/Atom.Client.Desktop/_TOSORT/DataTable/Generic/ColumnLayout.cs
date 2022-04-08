namespace Atom.Design.ObjectModel.DataTable.Generic
{
    public sealed class ColumnLayout
    {
        public ColumnLayout(string name, ITypeAdapter typeAdapter)
        {
            Name = name;
            Type = typeAdapter;
        }

        public string Name { get; private set; }

        public ITypeAdapter Type { get; private set; }
        
        public string ToString(object value)
        {
            return Type.ToString(value);
        }

        public bool TryParse(string value, out object parsed)
        {
            return Type.TryParse(value, out parsed);
        }
    }
}
