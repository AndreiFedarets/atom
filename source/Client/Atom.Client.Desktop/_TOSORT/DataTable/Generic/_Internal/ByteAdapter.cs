namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class ByteAdapter : BaseAdapter
    {
        public ByteAdapter()
            : base(typeof(byte))
        {
        }

        public override string ToString(object value)
        {
            return value.ToString();
        }

        public override bool TryParse(string value, out object parsed)
        {
            byte b;
            bool result = byte.TryParse(value, out b);
            parsed = b;
            return result;
        }
    }
}
