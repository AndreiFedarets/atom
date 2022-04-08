namespace Atom.Design.ObjectModel.DataTable.Generic
{
    public sealed class DataCell
    {
        private object _value;

        public DataCell(ColumnLayout cellType)
            : this(cellType, cellType.Type.DefaultValue)
        {
        }

        public DataCell(ColumnLayout layout, object value)
        {
            Layout = layout;
            _value = value;
        }

        public ColumnLayout Layout { get; private set; }

        public string Value
        {
            get { return Layout.ToString(_value); }
            set { Layout.TryParse(value, out _value); }
        }
    }
}
