using Atom.Design.Hosting;
using Atom.Design.Reflection;
using System.Collections.Generic;

namespace Atom.Design.Services
{
    public sealed class ObjectExplorer : IObjectExplorer
    {
        private readonly IAssemblyManager _assemblyManager;

        public ObjectExplorer(IAssemblyManager assemblyManager)
        {
            _assemblyManager = assemblyManager;
        }

        public List<IAction> GetAvailableActions(IProject project)
        {
            //TODO: use custom ActionCollection to monitor changes in Project.References to add and remove Assemblies
            List<IAction> actions = new List<IAction>();
            foreach (IAssembly assembly in _assemblyManager.GetAssemblies(project))
            {
                actions.AddRange(assembly.Actions.Values);
            }
            return actions;
        }

        public List<ITable> GetAvailableTables(IProject project)
        {
            //TODO: use custom ActionCollection to monitor changes in Project.References to add and remove Assemblies
            List<ITable> tables = new List<ITable>();
            foreach (IAssembly assembly in _assemblyManager.GetAssemblies(project))
            {
                tables.AddRange(assembly.Tables.Values);
            }
            return tables;
        }
    }
}
