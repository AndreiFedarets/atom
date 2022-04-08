using System.Collections.Generic;

namespace Atom.Design.Reflection
{
    public interface IValueScopeCollection : IValueScope
    {
        IEnumerable<IValueScope> Scopes { get; }
    }
}
