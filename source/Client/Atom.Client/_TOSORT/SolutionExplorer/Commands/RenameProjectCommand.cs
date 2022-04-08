using Atom.Design;

namespace Atom.Client.Win.SolutionExplorer.Commands
{
    public class RenameProjectCommand : ItemCommand
    {
        private readonly IWindowsManager _windowsManager;
        private readonly IProject _project;

        public RenameProjectCommand(IProject project, IWindowsManager windowsManager)
        {
            _project = project;
            _windowsManager = windowsManager;
        }

        public override string Header
        {
            get { return "Rename"; }
        }

        public override void Execute()
        {
            //TODO: show question
        }
    }
}
