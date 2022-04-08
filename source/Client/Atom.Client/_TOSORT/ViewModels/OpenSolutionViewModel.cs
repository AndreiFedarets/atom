using Atom.Design;
using Caliburn.Micro;

namespace Atom.Client.Win.ViewModels
{
    public class OpenSolutionViewModel : Screen
    {
        private readonly IWindowsManager _windowsManager;
        private readonly IApplication _application;
        private string _solutionFullName;

        public OpenSolutionViewModel(IApplication application, IWindowsManager windowsManager)
        {
            _application = application;
            _windowsManager = windowsManager;
            _solutionFullName = string.Empty;
        }

        public override string DisplayName
        {
            get { return "Open Solution"; }
            set { }
        }

        public string SolutionFullName
        {
            get { return _solutionFullName; }
            set
            {
                _solutionFullName = value;
                NotifyOfPropertyChange(() => SolutionFullName);
            }
        }

        public void BrowseSolution()
        {
            string solutionFileFullName;
            if (_windowsManager.TryOpenSolutionFile(out solutionFileFullName))
            {
                SolutionFullName = solutionFileFullName;
            }
        }

        public void OpenSolution()
        {
            _application.OpenSolution(_solutionFullName);
            TryClose(true);
        }
    }
}
