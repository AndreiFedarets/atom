using System.Linq;

namespace Atom.Design.Common
{
    public class DesignAssemblyResover : AssemblyResolver
    {
        public DesignAssemblyResover()
            : base(new[] { Environment.BasePath }.Concat(Environment.Extensions))
        {
        }
    }
}
