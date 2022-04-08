using Atom.Design.Hosting;
using Atom.Design.Reflection.Code.Services;
using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection.Code
{
    public sealed class ActionCollection : ObjectCollection<MethodReference, IAction>, IActionCollection
    {
        public ActionCollection(IProject project, ICodeParser codeParser)
            : base(project, codeParser)
        {
        }

        protected override MethodReference GetReference(IAction @object)
        {
            return @object.Reference;
        }
    }
}
