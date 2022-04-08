using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Extensibility
{
    internal sealed class CommonAssemblyLoader : IAssemblyLoader
    {
        private readonly List<IAssemblyLoader> _loaders;

        public CommonAssemblyLoader(IReflectionFacade reflection)
        {
            _loaders = new List<IAssemblyLoader>();
            _loaders.Add(new NativeAssemblyLoader(reflection));
            _loaders.Add(new ManagedAssemblyLoader());
        }

        public IAssembly LoadAssembly(string fileFullName)
        {
            IAssemblyLoader loader = FindFirstSupportedLoader(fileFullName);
            if (loader == null)
            {
                throw Fail.FileIsNotCorrectAssembly(fileFullName);
            }
            try
            {
                IAssembly assembly = loader.LoadAssembly(fileFullName);
                return assembly;
            }
            catch (Exception exception)
            {
                throw Fail.FileIsNotCorrectAssembly(fileFullName, exception);
            }
        }

        public bool IsValidAssemblyFile(string fileFullName)
        {
            try
            {
                return FindFirstSupportedLoader(fileFullName) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private IAssemblyLoader FindFirstSupportedLoader(string fileFullName)
        {
            IAssemblyLoader loader = _loaders.FirstOrDefault(x => x.IsValidAssemblyFile(fileFullName));
            return loader;
        }

        //private readonly List<IAssemblyLoader> _locators;
        //private readonly IReflectionFacade _reflection;

        //public CommonAssemblyLoader(IReflectionFacade reflection)
        //{
        //    _locators = new List<IAssemblyLoader>();
        //    _reflection = reflection;
        //    _locators.Add(new CodedActionLocator(reflection));
        //    _locators.Add(new DeclaredActionLocator());
        //}

        //public IEnumerable<IActionType> LoadActions(IActionAssembly assembly)
        //{
        //    List<IActionType> actions = new List<IActionType>();
        //    foreach (IAssemblyLoader locator in _locators)
        //    {
        //        actions.AddRange(locator.LoadActions(assembly));
        //    }
        //    return actions;
        //}

        //public IEnumerable<IActionAssembly> LoadAssemblies(IDirectories directories)
        //{
        //    List<IActionAssembly> assemblies = new List<IActionAssembly>();
        //    foreach (DirectoryInfo directoryInfo in directories.ModulesDirectories)
        //    {
        //        if (directoryInfo.Exists)
        //        {
        //            LoadAssembliesFromDirectory(directoryInfo, assemblies);
        //        }
        //    }
        //    return assemblies;
        //}

        //private void LoadAssembliesFromDirectory(DirectoryInfo directoryInfo, List<IActionAssembly> assemblies)
        //{
        //    FileInfo[] fileInfos = directoryInfo.GetFiles("*.dll");
        //    foreach (FileInfo fileInfo in fileInfos)
        //    {
        //        LoadAssembliesFromFile(fileInfo, assemblies);
        //    }
        //}

        //private void LoadAssembliesFromFile(FileInfo fileInfo, List<IActionAssembly> assemblies)
        //{
        //    try
        //    {
        //        Assembly assembly = RuntimeAssemblyCache.LoadFrom(fileInfo.FullName);
        //        if (assembly == null)
        //        {
        //            return;
        //        }
        //        if (!assembly.GetCustomAttributes(typeof(ActionAssemblyAttribute), true).Any())
        //        {
        //            return;
        //        }
        //        ActionAssembly module = new ActionAssembly(assembly, this, _reflection);
        //        assemblies.Add(module);
        //    }
        //    catch (Exception exception)
        //    {
        //        LoggingProvider.Current.Log(TraceEventType.Information, exception);
        //    }
        //}
    }
}
