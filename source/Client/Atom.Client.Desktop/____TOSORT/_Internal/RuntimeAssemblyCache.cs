using System.IO;
using System.Reflection;

namespace Atom
{
    internal static class RuntimeAssemblyCache
    {
        //private static readonly Dictionary<string, Assembly> Assemblies;



        public static Assembly LoadFrom(string fileFullName)
        {
            fileFullName = Path.GetFullPath(fileFullName);
            return Assembly.LoadFrom(fileFullName);
        }
    }
}
