using System.Windows.Input;
using Atom.Client.Win.Commands;
using Atom.Design;
using Caliburn.Micro;

namespace Atom.Client.Win.ViewModels
{
    public sealed class SolutionViewModel : Conductor<ViewModel>.Collection.OneActive
    {
        private readonly ISolution _solution;

        public SolutionViewModel(ISolution solution, IViewModelManager viewModelManager)
        {
            _solution = solution;
            CloseDocumentCommand = new SyncCommand<ViewModel>(CloseDocument);
        }

        public ICommand CloseDocumentCommand { get; private set; }

        public void CloseDocument(ViewModel viewModel)
        {
            if (viewModel != null)
            {
                DeactivateItem(viewModel, true);
            }
        }
    }
}
