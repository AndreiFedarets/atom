using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;

namespace Atom.Design.Reflection
{
    public interface IActionCollection : IReadOnlyDictionary<MethodReference, IAction>
    {
    }
}
