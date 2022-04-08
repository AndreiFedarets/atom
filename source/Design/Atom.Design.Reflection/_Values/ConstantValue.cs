using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public class ConstantValue : BaseValue
    {
        public ConstantValue(TypeReference type, object value)
            : base(type)
        {
            Value = value;
        }

        public object Value { get; private set; }
    }
}
