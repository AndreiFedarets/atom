using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public interface ITableValue : IValueSource
    {
        PropertyReference Property { get; }
    }
}
