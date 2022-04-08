namespace Atom.Runtime.Extension.Desktop
{
    public sealed class ControlProperty
    {
        public ControlProperty(int id, object value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; private set; }

        public object Value { get; private set; }
    }
}
