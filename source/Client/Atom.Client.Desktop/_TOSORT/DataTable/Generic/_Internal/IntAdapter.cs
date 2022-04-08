namespace Atom.Design.ObjectModel.DataTable.Generic
{
    internal sealed class IntAdapter : BaseAdapter
    {
        public IntAdapter()
            : base(typeof(int), default(int))
        {
        }
        
        public override bool TryParse(string value, out object parsed)
        {
            int i;
            bool result = int.TryParse(value, out i);
            parsed = i;
            return result;
        }
    }
}
