namespace Atom.Extensibility
{
    internal interface IAssemblyLoader
    {
        IAssembly LoadAssembly(string fileFullName);

        bool IsValidAssemblyFile(string fileFullName);
    }
}
