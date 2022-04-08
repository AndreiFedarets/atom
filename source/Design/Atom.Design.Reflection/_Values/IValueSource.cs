using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public interface IValueSource
    {
        string ValueName { get; }

        TypeReference ValueType { get; }

        BaseValue CreateValue();
    }
}
