using System;
using System.Collections.Generic;

namespace Atom
{
    internal sealed class ActionTypeCollection : ReadOnlyDictionary<Guid, IActionType>, IActionTypeCollection
    {
        public ActionTypeCollection(IDictionary<Guid, IActionType> actions)
            : base(actions)
        {
        }
    }
}
