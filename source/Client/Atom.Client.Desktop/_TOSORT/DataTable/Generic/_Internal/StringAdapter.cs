namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class StringAdapter : BaseAdapter
    {
        public StringAdapter()
            : base(typeof(string), string.Empty)
        {
        }
        
        public override bool TryParse(string value, out object parsed)
        {
            parsed = value;
            return true;
        }
    }
}
