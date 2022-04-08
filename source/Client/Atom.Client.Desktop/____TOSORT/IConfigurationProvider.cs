using Atom.Configuration;

namespace Atom
{
    public interface IConfigurationProvider
    {
        ModulesSection Modules { get; }
    }
}
