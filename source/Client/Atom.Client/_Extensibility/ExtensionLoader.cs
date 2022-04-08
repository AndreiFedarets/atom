using Atom.Design.Services;
using Layex;
using System;
using System.IO;
using System.Reflection;

namespace Atom.Client
{
    internal sealed class ExtensionLoader
    {
        private readonly ITypeService _typeService;
        private readonly IDependencyContainer _container;

        public ExtensionLoader(IDependencyContainer container, ITypeService typeService)
        {
            _container = container;
            _typeService = typeService;
        }

        public void LoadExtensions()
        {
            foreach (string extensionPath in Design.Common.Environment.Extensions)
            {
                LoadExtension(extensionPath);
            }
        }

        private void LoadExtension(string extensionPath)
        {
            foreach (string assemblyFile in Directory.GetFiles(extensionPath, "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyFile);
                    LoadExtension(assembly);
                }
                catch (Exception)
                {
                    //TODO: log exception
                }
            }
        }

        private void LoadExtension(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ITypeExtension).IsAssignableFrom(type))
                {
                    ITypeExtension extension = (ITypeExtension)_container.Resolve(type);
                    _typeService.RegisterExtension(extension);
                }
            }
        }
    }
}
