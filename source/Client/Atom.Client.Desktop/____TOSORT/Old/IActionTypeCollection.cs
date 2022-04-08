using System;

namespace Atom
{
    public interface IActionTypeCollection : IReadOnlyDictionary<Guid, IActionType>
    {
    }
}
