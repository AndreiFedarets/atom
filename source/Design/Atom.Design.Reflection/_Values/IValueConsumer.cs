using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public interface IValueConsumer
    {
        BaseValue Value { get; set; }

        TypeReference ValueType { get; }
    }
}
