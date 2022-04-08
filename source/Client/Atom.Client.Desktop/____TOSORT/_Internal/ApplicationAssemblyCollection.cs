namespace Atom
{
    internal sealed class ApplicationAssemblyCollection : ReadOnlyCollection<IAssembly>, IAssemblyCollection
    {
        internal void Load(IAssemblyManager manager)
        {
            foreach (IAssembly assembly in manager.EnumerateAssemblies())
            {
                AddInternal(assembly);
            }
        }
    }
}
