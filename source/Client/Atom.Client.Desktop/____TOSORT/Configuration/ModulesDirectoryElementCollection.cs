using System.Configuration;

namespace Atom.Configuration
{
    public class ModulesDirectoryElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModulesDirectoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModulesDirectoryElement)element).Path.ToLowerInvariant();
        }
    }
}
