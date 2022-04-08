using Caliburn.Micro;

namespace Atom.Client.Win.ViewModels
{
    public sealed class ToolsViewModel : Conductor<Screen>.Collection.OneActive
    {
        public ToolsViewModel(IApplication application, IViewModelManager viewsManager)
        {
            ActivateItem(viewsManager.ResolveSolutionExplorerViewModel(application.CurrentSolution));
        }
    }
}
