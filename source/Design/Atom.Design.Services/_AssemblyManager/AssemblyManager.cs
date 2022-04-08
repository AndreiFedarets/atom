using Atom.Design.Hosting;
using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;

namespace Atom.Design.Services
{
    internal sealed class AssemblyManager : IAssemblyManager
    {
        private readonly Dictionary<string, IAssembly> _assemblyCache;
        private List<IAssemblyLoader> _assemblyLoaders;

        public AssemblyManager()
        {
            _assemblyLoaders = new List<IAssemblyLoader>();
            _assemblyCache = new Dictionary<string, IAssembly>();
        }

        public void AddLoader(IAssemblyLoader assemblyLoader)
        {
            _assemblyLoaders.Add(assemblyLoader);
        }

        public IAssembly GetAssembly(AssemblyReference assemblyReference, IProject context)
        {
            IAssembly assembly = null;
            string assemblyFile = ResolveAssembly(assemblyReference, context);
            if (!string.IsNullOrEmpty(assemblyFile))
            {
                assembly = GetAssembly(assemblyFile);
            }
            return assembly;
        }

        private IAssembly GetAssembly(string assemblyFile)
        {
            IAssembly assembly = null;
            string assemblyKey = assemblyFile.ToLowerInvariant();
            lock (_assemblyCache)
            {
                if (!_assemblyCache.TryGetValue(assemblyKey, out assembly))
                {
                    assembly = LoadAssembly(assemblyFile);
                    _assemblyCache.Add(assemblyKey, assembly);
                }
            }
            return assembly;
        }

        private string ResolveAssembly(AssemblyReference assemblyReference, IProject context)
        {
            AssemblyReference currentAssemblyReference = new AssemblyReference(context.AssemblyName);
            if (assemblyReference.Equals(currentAssemblyReference))
            {
                return context.Name;
            }
            foreach (IReference reference in context.References)
            {
                currentAssemblyReference = new AssemblyReference(reference.AssemblyName);
                if (assemblyReference.Equals(currentAssemblyReference))
                {
                    return reference.AssemblyFile;
                }
            }
            return string.Empty;
        }

        private IAssembly LoadAssembly(string assemblyFile)
        {
            IAssembly assembly = null;
            foreach (IAssemblyLoader assemblyManager in _assemblyLoaders)
            {
                assembly = assemblyManager.LoadAssembly(assemblyFile);
                if (assembly != null)
                {
                    break;
                }
            }
            return assembly;
        }

        public IEnumerable<IAssembly> GetAssemblies(IProject project)
        {
            List<IAssembly> assemblies = new List<IAssembly>();
            IAssembly projectAssembly = GetAssembly(project.Name);
            assemblies.Add(projectAssembly);
            foreach (IReference reference in project.References)
            {
                IAssembly assembly = GetAssembly(reference.AssemblyFile);
                if (assembly != null)
                {
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }
    }
}
