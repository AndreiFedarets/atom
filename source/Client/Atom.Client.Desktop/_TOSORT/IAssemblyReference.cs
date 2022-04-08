using Atom.Metadata;

namespace Atom.Design
{
    public interface IAssemblyReference
    {
        AssemblyMetadata Metadata { get; }

        bool EmbedAssembly { get; set; }
    }
}
