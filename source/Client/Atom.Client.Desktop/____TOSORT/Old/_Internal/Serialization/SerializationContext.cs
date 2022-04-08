namespace Atom.Serialization
{
    internal sealed class SerializationContext
    {
        public SerializationContext(IScope applicationScope, IActionTreeWalker treeWalker, IAssemblyManager assemblyManager, IAssembly assembly)
        {
            ApplicationScope = applicationScope;
            TreeWalker = treeWalker;
            AssemblyManager = assemblyManager;
            Assembly = assembly;
        }

        public IScope ApplicationScope { get; private set; }

        public IActionTreeWalker TreeWalker { get; private set; }

        public IAssembly Assembly { get; private set; }

        public IAssemblyManager AssemblyManager { get; private set; }
    }
}
