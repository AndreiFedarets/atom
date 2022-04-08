using Atom.Design;
using Caliburn.Micro;

namespace Atom.Client.Win.ViewModels
{
    public class CreateSolutionViewModel : Screen
    {
        private readonly IApplication _application;
        private string _solutionName;
        private string _solutionPath;

        public CreateSolutionViewModel(IApplication application)
        {
            _application = application;
            _solutionName = Properties.Resources.SolutionDefaultName;
            _solutionPath = _application.Directories.SolutionsDefaultDirectory.FullName;
        }

        public override string DisplayName
        {
            get { return "Create Solution"; }
            set { }
        }

        public string SolutionName
        {
            get { return _solutionName; }
            set
            {
                _solutionName = value;
                NotifyOfPropertyChange(() => SolutionName);
            }
        }

        public string SolutionPath
        {
            get { return _solutionPath; }
            set
            {
                _solutionPath = value;
                NotifyOfPropertyChange(() => SolutionPath);
            }
        }

        public void CreateSolution()
        {
            _application.CreateSolution(_solutionPath, _solutionName);
            TryClose(true);
        }
    }
}
