using System.Reflection;

namespace Atom.Design.Hosting
{
    public interface IReference
    {
        AssemblyName AssemblyName { get; }

        string AssemblyFile { get; }
    }
}
