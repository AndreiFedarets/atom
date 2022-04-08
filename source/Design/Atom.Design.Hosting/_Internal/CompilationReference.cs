using System.Reflection;

namespace Atom.Design.Hosting
{
    internal sealed class CompilationReference : IReference
    {
        private readonly Microsoft.CodeAnalysis.CompilationReference _reference;

        public CompilationReference(Microsoft.CodeAnalysis.CompilationReference reference)
        {
            _reference = reference;
        }

        public AssemblyName AssemblyName
        {
            get
            {
                Microsoft.CodeAnalysis.Compilation compilation = _reference.Compilation;
                Microsoft.CodeAnalysis.AssemblyIdentity identity = compilation.Assembly.Identity;
                return new AssemblyName(identity.ToString());
            }
        }

        public string AssemblyFile
        {
            get { return _reference.Display; }
        }
    }
}
