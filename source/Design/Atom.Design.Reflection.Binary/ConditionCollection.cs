using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design.Reflection.Binary
{
    [Serializable]
    public sealed class ConditionCollection : ReadOnlyDictionary<MethodReference, ICondition>, IConditionCollection
    {
        public ConditionCollection(Dictionary<MethodReference, ICondition> collection)
            : base(collection)
        {
        }
    }
}
