using System;

namespace Atom.Design.Reflection.Binary
{
    public sealed class AssemblyLoader : IAssemblyLoader
    {
        public IAssembly LoadAssembly(string assemblyFile)
        {
            IAssembly assembly = null;
            try
            {
                using (SandboxAppDomain sandbox = new SandboxAppDomain())
                {
                    AssemblyInternalLoader assemblyLoader = sandbox.CreateInstance<AssemblyInternalLoader>();
                    assembly = assemblyLoader.LoadAssembly(assemblyFile);
                }
            }
            catch (Exception)
            {
                //TODO: log exception
            }
            return assembly;
        }
    }
}
