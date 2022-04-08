namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class UnsupportedAdapter : BaseAdapter
    {
        public UnsupportedAdapter()
            : base(typeof(object), null)
        {

        }

        public override string ToString(object value)
        {
            return null;
        }

        public override bool TryParse(string value, out object parsed)
        {
            parsed = null;
            return false;
        }
    }
}
