using Atom.Design.Hosting;
using Atom.Design.Reflection.Code.Services;
using System;

namespace Atom.Design.Reflection.Code
{
    public sealed class AssemblyLoader : IAssemblyLoader
    {
        private readonly IWorkspace _workspace;
        private readonly ICodeParser _codeParser;

        public AssemblyLoader(IWorkspace workspace)
        {
            _workspace = workspace;
            _codeParser = new CommonCodeParser();
        }

        public IAssembly LoadAssembly(string assemblyFile)
        {
            foreach (IProject project in _workspace.Solution.Projects)
            {
                if (string.Equals(project.Name, assemblyFile, StringComparison.OrdinalIgnoreCase))
                {
                     return new Assembly(project, _codeParser);
                }
            }
            return null;
        }
    }
}
