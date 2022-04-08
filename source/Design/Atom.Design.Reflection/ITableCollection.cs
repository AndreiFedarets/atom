using System.Collections.Generic;
using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public interface ITableCollection : IReadOnlyDictionary<TypeReference, ITable>
    {
    }
}
