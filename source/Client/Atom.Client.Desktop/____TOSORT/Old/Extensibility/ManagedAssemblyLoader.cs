namespace Atom.Extensibility
{
    internal sealed class ManagedAssemblyLoader : IAssemblyLoader
    {
        public IAssembly LoadAssembly(string fileFullName)
        {
            return null;
        }

        public bool IsValidAssemblyFile(string fileFullName)
        {
            return false;
        }
    }
}
