using Atom.Metadata;

namespace Atom.Design
{
    internal sealed class AssemblyReference : IAssemblyReference
    {
        public AssemblyReference(AssemblyMetadata assemblyMetadata, bool embedAssembly)
        {
            Metadata = assemblyMetadata;
            EmbedAssembly = embedAssembly;
        }

        public AssemblyMetadata Metadata { get; private set; }

        public bool EmbedAssembly { get; set; }
    }
}
