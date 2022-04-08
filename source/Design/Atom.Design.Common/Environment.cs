using System.Collections.Generic;
using System.IO;

namespace Atom.Design.Common
{
    public static class Environment
    {
        private const string InternalExecutorFileName = "Atom.Runtime.Executor.exe";
        private const string ExtensionsDirectoryName = "Extensions";
        private const string AtomDirectoryEnvironmentVariable = "%AtomDir%";

        static Environment()
        {
            BasePath = System.Environment.ExpandEnvironmentVariables(AtomDirectoryEnvironmentVariable);
            InternalExecutorPath = Path.Combine(BasePath, InternalExecutorFileName);
        }

        public static string InternalExecutorPath { get; private set; }

        public static string BasePath { get; private set; }

        public static IEnumerable<string> Extensions
        {
            get
            {
                string extensionsPath = Path.Combine(BasePath, ExtensionsDirectoryName);
                foreach (string extensionDirectory in Directory.GetDirectories(extensionsPath))
                {
                    yield return extensionDirectory;
                }
            }
        }
    }
}
