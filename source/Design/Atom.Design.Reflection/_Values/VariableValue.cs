using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public class VariableValue : BaseValue
    {
        public VariableValue(TypeReference type, string name)
            : base(type)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
