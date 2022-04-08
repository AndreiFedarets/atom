using Atom.Design.Hosting;
using Atom.Design.Reflection.Code.Services;
using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection.Code
{
    public sealed class TableCollection : ObjectCollection<TypeReference, ITable>, ITableCollection
    {
        public TableCollection(IProject project, ICodeParser codeParser)
            : base(project, codeParser)
        {
        }

        protected override TypeReference GetReference(ITable @object)
        {
            return @object.Reference;
        }
    }
}
