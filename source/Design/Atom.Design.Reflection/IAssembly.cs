using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public interface IAssembly
    {
        AssemblyReference Reference { get; }

        IActionCollection Actions { get; }

        IConditionCollection Conditions { get; }

        ITableCollection Tables { get; }
    }
}
