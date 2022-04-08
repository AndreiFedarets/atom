using Atom.Design;

namespace Atom.Client.Win.SolutionExplorer.Commands
{
    public class RemoveReferenceCommand : ItemCommand
    {
        private readonly IWindowsManager _windowsManager;
        private readonly IAssemblyReference _reference;
        private readonly IAssemblyReferenceCollection _references;

        public RemoveReferenceCommand(IAssemblyReference reference, IAssemblyReferenceCollection references, IWindowsManager windowsManager)
        {
            _reference = reference;
            _references = references;
            _windowsManager = windowsManager;
        }

        public override string Header
        {
            get { return "Remove Reference"; }
        }

        public override void Execute()
        {
            //TODO: show question
            _references.Remove(_reference);
        }
    }
}
