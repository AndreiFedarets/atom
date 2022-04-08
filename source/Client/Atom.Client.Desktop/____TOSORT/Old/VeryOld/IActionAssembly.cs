using System;
using System.Reflection;

namespace Atom
{
    public interface IActionAssembly
    {
        Guid Uid { get; }

        AssemblyName Name { get; }

        Assembly Assembly { get; }

        IActionTypeCollection Actions { get; }
    }
}
