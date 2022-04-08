using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design.Reflection.Binary
{
    [Serializable]
    public sealed class ActionCollection : ReadOnlyDictionary<MethodReference, IAction>, IActionCollection
    {
        public ActionCollection(Dictionary<MethodReference, IAction> collection)
            : base(collection)
        {
        }
    }
}
