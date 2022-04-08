using System;

namespace Atom
{
    public interface IActionAssemblyCollection : IReadOnlyCollection<IActionAssembly>
    {
        IActionAssembly this[Guid uid] { get; }

        bool Contains(Guid uid);
    }
}
