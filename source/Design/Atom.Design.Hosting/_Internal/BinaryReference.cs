using System.Reflection;

namespace Atom.Design.Hosting
{
    internal sealed class BinaryReference : IReference
    {
        private readonly Microsoft.CodeAnalysis.MetadataReference _reference;

        public BinaryReference(Microsoft.CodeAnalysis.MetadataReference reference)
        {
            _reference = reference;
        }

        public AssemblyName AssemblyName
        {
            get { return AssemblyName.GetAssemblyName(AssemblyFile); }
        }

        public string AssemblyFile
        {
            get { return _reference.Display; }
        }
    }
}
