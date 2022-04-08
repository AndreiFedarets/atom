using System.Collections.Generic;

namespace Atom.Design.Services
{
    public interface ITypeExtension
    {
        string DisplayName { get; }

        IEnumerable<TypeAdapter> Types { get; }

        void InsertInteractive(Table table);

        object CreateInteractive();

        object EditInteractive(object value);
    }
}
