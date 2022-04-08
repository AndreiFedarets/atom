using Atom.Design;

namespace Atom.Client.Win.SolutionExplorer.Commands
{
    public class CreateProjectCommand : ItemCommand
    {
        private readonly IWindowsManager _windowsManager;
        private readonly ISolution _solution;

        public CreateProjectCommand(ISolution solution, IWindowsManager windowsManager)
        {
            _solution = solution;
            _windowsManager = windowsManager;
        }

        public override string Header
        {
            get { return "Create Project"; }
        }

        public override void Execute()
        {
            _windowsManager.ShowCreateProject(_solution);
        }
    }
}
