using System.Configuration;

namespace Atom.Configuration
{
    public class ModulesDirectoryElement : ConfigurationElement
    {
        private const string PathPropertyName = "path";

        [ConfigurationProperty(PathPropertyName, IsRequired = true)]
        public string Path
        {
            get { return (string)this[PathPropertyName]; }
            set { this[PathPropertyName] = value; }
        }
    }
}
