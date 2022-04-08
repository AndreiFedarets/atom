using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;

namespace Atom.Design.Reflection
{
    public interface ITable : IReadOnlyDictionary<string, ITableValue>, IValueScope
    {
        string Title { get; }

        TypeReference Reference { get; }
    }
}
