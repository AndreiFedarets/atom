using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;
using Layex.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Client.Desktop.ViewModels
{
    public sealed class SolutionExplorerViewModel : ViewModel
    {
        private readonly IWorkspace _workspace;
        private readonly ObservableCollection<SolutionTree.ItemViewModel> _items;
        private readonly IViewManager _viewManager;
        private readonly IDesignerSerializer _designerSerializer;
        private SolutionTree.ItemViewModel _selectedItem;

        public SolutionExplorerViewModel(IWorkspace workspace, IViewManager viewManager, IDesignerSerializer designerSerializer)
        {
            _workspace = workspace;
            _viewManager = viewManager;
            _designerSerializer = designerSerializer;
            _items = new ObservableCollection<SolutionTree.ItemViewModel>();
            _workspace.SolutionOpened += OnSolutionOpened;
            _workspace.SolutionClosed += OnSolutionClosed;
        }

        public IEnumerable<SolutionTree.ItemViewModel> Items
        {
            get { return _items; }
        }

        public SolutionTree.ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public void OpenSelectedItem()
        {
            SolutionTree.FileViewModel fileViewModel = SelectedItem as SolutionTree.FileViewModel;
            if (fileViewModel != null)
            {
                _viewManager.OpenDocument(fileViewModel);
            }
        }

        private void OnSolutionClosed(object sender, System.EventArgs e)
        {
            _items.Clear();
        }

        private void OnSolutionOpened(object sender, System.EventArgs e)
        {
            SolutionTree.SolutionViewModel solution = new SolutionTree.SolutionViewModel(_workspace.Solution, new SolutionTree.SolutionItemFilter());
            _items.Add(solution);
        }
    }
}
