using Atom.Configuration;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;

namespace Atom
{
    public sealed class Directories : IDirectories
    {
        public const string SolutionsDirectoryName = "MGS Atom";
        private readonly IConfigurationProvider _configurationProvider;

        public Directories(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            string temp = Path.Combine(UserDocumentsPath, SolutionsDirectoryName);
            SolutionsDefaultDirectory = new DirectoryInfo(temp);
            if (!SolutionsDefaultDirectory.Exists)
            {
                SolutionsDefaultDirectory.Create();
            }
        }

        public Directories()
            : this(ConfigurationProvider.Current)
        {
        }

        public IEnumerable<DirectoryInfo> ModulesDirectories
        {
            get
            {
                List<DirectoryInfo> directories = new List<DirectoryInfo>();
                ModulesDirectoryElementCollection collection = _configurationProvider.Modules.Directories;
                foreach (ModulesDirectoryElement element in collection)
                {
                    string absolutePath = ResolvePath(element.Path);
                    DirectoryInfo directoryInfo = new DirectoryInfo(absolutePath);
                    directories.Add(directoryInfo);
                }
                return directories;
            }
        }

        public DirectoryInfo SolutionsDefaultDirectory { get; private set; }

        public string UserDocumentsPath
        {
            get
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"))
                {
                    return (string)key.GetValue("Personal");
                }
            }
        }

        private string ResolvePath(string relativePath)
        {
            return Path.GetFullPath(relativePath);
        }
    }
}
