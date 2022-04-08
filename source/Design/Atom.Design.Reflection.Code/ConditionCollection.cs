using Atom.Design.Hosting;
using Atom.Design.Reflection.Code.Services;
using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection.Code
{
    public sealed class ConditionCollection : ObjectCollection<MethodReference, ICondition>, IConditionCollection
    {
        public ConditionCollection(IProject project, ICodeParser codeParser)
            : base(project, codeParser)
        {
        }

        protected override MethodReference GetReference(ICondition @object)
        {
            return @object.Reference;
        }
    }
}
