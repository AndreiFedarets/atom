using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public abstract class BaseValue
    {
        public BaseValue(TypeReference type)
        {
            Type = type;
        }

        public TypeReference Type { get; private set; }
    }
}
