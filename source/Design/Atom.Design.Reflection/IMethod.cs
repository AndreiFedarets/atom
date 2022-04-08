using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public interface IMethod
    {
        string Title { get; }

        MethodReference Reference { get; }
    }
}
