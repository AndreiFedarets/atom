namespace Atom.Design.Reflection
{
    public interface IAssemblyLoader
    {
        IAssembly LoadAssembly(string assemblyFile);
    }
}