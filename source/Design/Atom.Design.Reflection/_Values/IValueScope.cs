using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Reflection
{
    public interface IValueScope
    {
        IEnumerable<IValueSource> Sources { get; }

        IEnumerable<IValueConsumer> Consumers { get; }
    }

    public static class ValueScopeExtensions
    {
        public static IEnumerable<IValueSource> GetSources(this IValueScope scope, TypeReference targetType)
        {
            return scope.Sources.Where(x => targetType.IsAssignableFrom(x.ValueType));
        }
    }
}
