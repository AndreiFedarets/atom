using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Atom.Design.Common
{
    public class AssemblyResolver : MarshalByRefObject, IDisposable
    {
        private string[] _probingPaths;
        private Dictionary<string, Assembly> _assemblyCache;

        public AssemblyResolver(IEnumerable<string> probingPaths)
        {
            _probingPaths = probingPaths.ToArray();
            _assemblyCache = new Dictionary<string, Assembly>();
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        ~AssemblyResolver()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_assemblyCache)
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
                    _assemblyCache.Clear();
                }
            }
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyKey = args.Name.ToLowerInvariant();
            Assembly assembly = null;
            lock (_assemblyCache)
            {
                if (!_assemblyCache.TryGetValue(assemblyKey, out assembly))
                {
                    AssemblyName assemblyName = new AssemblyName(args.Name);
                    assembly = LoadAssemblySafe(assemblyName);
                    _assemblyCache[assemblyKey] = assembly;
                }
            }
            return assembly;
        }

        private Assembly LoadAssemblySafe(AssemblyName assemblyName)
        {
            Assembly assembly = null;
            string assemblyFullName = FindAssembly(assemblyName);
            if (!string.IsNullOrEmpty(assemblyFullName))
            {
                try
                {
                    assembly = Assembly.LoadFile(assemblyFullName);
                }
                catch (Exception)
                {
                    //TODO: Log Exception
                }
            }
            return assembly;
        }

        private string FindAssembly(AssemblyName requestedAssemblyName)
        {
            foreach (string probingPath in _probingPaths)
            {
                foreach (string assemblyFile in Directory.GetFiles(probingPath, "*.dll"))
                {
                    AssemblyName assemblyName = null;
                    try
                    {
                        assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
                    }
                    catch (Exception)
                    {
                    }
                    if (assemblyName != null && AreAssembliesCompatible(requestedAssemblyName, assemblyName))
                    {
                        return assemblyFile;
                    }
                }
            }
            return string.Empty;
        }

        private bool AreAssembliesCompatible(AssemblyName requestedAssemblyName, AssemblyName currentAssemblyName)
        {
            if (!string.Equals(requestedAssemblyName.Name, currentAssemblyName.Name))
            {
                return false;
            }
            if (currentAssemblyName.Version < requestedAssemblyName.Version)
            {
                return false;
            }
            return true;
        }
    }
}
