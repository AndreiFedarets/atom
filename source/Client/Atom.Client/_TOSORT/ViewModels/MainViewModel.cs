using System.Windows.Input;
using Atom.Client.Win.Commands;
using Atom.Design;

namespace Atom.Client.Win.ViewModels
{
    public sealed class MainViewModel : ViewModel
    {
        private readonly IApplication _application;
        private readonly IWindowsManager _windowsManager;
        private readonly IViewModelManager _viewModelManager;
        private SolutionViewModel _solutionViewModel;

        public MainViewModel(IApplication application, IWindowsManager windowsManager, IViewModelManager viewModelManager)
        {
            _application = application;
            _windowsManager = windowsManager;
            _viewModelManager = viewModelManager;
            _application.SolutionOpened += OnSolutionOpened;
            CreateSolutionCommand = new SyncCommand<ISolution>(CreateSolution, CanCreateSolution);
            OpenSolutionCommand = new SyncCommand<ISolution>(OpenSolution, CanOpenSolution);
            SaveSolutionCommand = new SyncCommand<ISolution>(SaveSolution, CanSaveSolution);
            CloseSolutionCommand = new SyncCommand<ISolution>(CloseSolution, CanCloseSolution);
            ShowActionStoreCommand = new SyncCommand<ISolution>(ShowActionStore, CanShowActionStore);
            ShowSolutionExplorerCommand = new SyncCommand<ISolution>(ShowSolutionExplorer, CanShowSolutionExplorer);
        }

        public ISolution Solution
        {
            get { return _application.CurrentSolution; }
        }

        public SolutionViewModel SolutionViewModel
        {
            get { return _solutionViewModel; }
            private set
            {
                _solutionViewModel = value;
                NotifyOfPropertyChange(() => SolutionViewModel);
            }
        }

        public override string DisplayName
        {
            get { return "Atom"; }
            set { }
        }

        public ICommand CreateSolutionCommand { get; private set; }

        public ICommand OpenSolutionCommand { get; private set; }

        public ICommand SaveSolutionCommand { get; private set; }

        public ICommand CloseSolutionCommand { get; private set; }

        public ICommand ShowActionStoreCommand { get; private set; }

        public ICommand ShowSolutionExplorerCommand { get; private set; }

        private void CreateSolution(ISolution solution)
        {
            _windowsManager.ShowCreateSolution();
        }

        private bool CanCreateSolution(ISolution solution)
        {
            return _application.CurrentSolution == null;
        }

        private void OpenSolution(ISolution solution)
        {
            _windowsManager.ShowOpenSolution();
        }

        private void SaveSolution(ISolution solution)
        {
            _application.SaveSolution();
        }

        private bool CanSaveSolution(ISolution solution)
        {
            return _application.CurrentSolution != null;
        }

        private bool CanOpenSolution(ISolution solution)
        {
            return _application.CurrentSolution == null;
        }

        private void CloseSolution(ISolution solution)
        {
            _application.CloseSolution();
        }

        private bool CanCloseSolution(ISolution solution)
        {
            return _application.CurrentSolution != null;
        }

        private void ShowActionStore(ISolution solution)
        {
            _windowsManager.ShowExtensionsExplorer();
        }

        private bool CanShowActionStore(ISolution solution)
        {
            return _application.CurrentSolution != null;
        }

        private void ShowSolutionExplorer(ISolution solution)
        {
            _windowsManager.ShowSolutionExplorer(_application.CurrentSolution);
        }

        private bool CanShowSolutionExplorer(ISolution solution)
        {
            return _application.CurrentSolution != null;
        }

        private void OnSolutionOpened(object sender, SolutionEventArgs e)
        {
            SolutionViewModel = _viewModelManager.ResolveSolutionViewModel(_application.CurrentSolution);
            NotifyOfPropertyChange(() => Solution);
        }
    }
}
