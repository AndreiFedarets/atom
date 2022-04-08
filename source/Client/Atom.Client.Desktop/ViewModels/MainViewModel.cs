using Atom.Design.Hosting;
using Layex.ViewModels;
using System.Windows;

namespace Atom.Client.Desktop.ViewModels
{
    public sealed class MainViewModel : ItemsViewModel
    {
        private readonly IViewManager _viewManager;
        private readonly IWorkspace _workspace;

        public MainViewModel(IWorkspace workspace, IViewManager viewManager)
        {
            _workspace = workspace;
            _viewManager = viewManager;
        }

        public void OpenSolution()
        {
            string solutionFileFullName = _viewManager.BrowseSolution();
            if (!string.IsNullOrEmpty(solutionFileFullName))
            {
                _workspace.OpenSolution(solutionFileFullName);
            }
        }

        public void Test()
        {
            //if (_workspace.Solution.Projects.Count() == 0)
            //{
            //    _workspace.OpenSolution(@"C:\Users\Andrew\source\repos\Atom.ActionLibrary3\Atom.ActionLibrary3.sln");
            //}
            //_viewManager.ShowActionExplorer(_workspace.Solution.Projects.FirstOrDefault());
        }
    }
}
