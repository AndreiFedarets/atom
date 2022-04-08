using Atom.Design;

namespace Atom.Client.Win.SolutionExplorer.Commands
{
    public class RenameSolutionCommand : ItemCommand
    {
        private readonly IWindowsManager _windowsManager;
        private readonly ISolution _solution;

        public RenameSolutionCommand(ISolution solution, IWindowsManager windowsManager)
        {
            _solution = solution;
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
