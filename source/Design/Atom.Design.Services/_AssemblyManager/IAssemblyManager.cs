using Atom.Design.Hosting;
using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;

namespace Atom.Design.Services
{
    public interface IAssemblyManager
    {
        IAssembly GetAssembly(AssemblyReference assemblyReference, IProject context);

        IEnumerable<IAssembly> GetAssemblies(IProject project);
    }
}
