using System.Configuration;

namespace Atom.Configuration
{
    public class ModulesSection : ConfigurationSection
    {
        private const string DirectoriesPropertyName = "directories";

        [ConfigurationProperty(DirectoriesPropertyName, IsRequired = false)]
        public ModulesDirectoryElementCollection Directories
        {
            get { return (ModulesDirectoryElementCollection)this[DirectoriesPropertyName]; }
            set { this[DirectoriesPropertyName] = value; }
        }

    }
}
