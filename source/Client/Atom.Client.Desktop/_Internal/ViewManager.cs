using Atom.Client.Desktop.ViewModels;
using Atom.Client.Desktop.ViewModels.SolutionTree;
using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;
using Layex.ViewModels;
using Microsoft.Win32;

namespace Atom.Client.Desktop
{
    public class ViewManager : IViewManager
    {
        private readonly IViewModelManager _viewModelManager;
        private readonly IDesignerSerializer _designerSerializer;

        public ViewManager(IViewModelManager viewModelManager, IDesignerSerializer designerSerializer)
        {
            _viewModelManager = viewModelManager;
            _designerSerializer = designerSerializer;
        }

        public string BrowseSolution()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Solution files (*.sln)|*.sln";
            bool? result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return openFileDialog.FileName;
            }
            return string.Empty;
        }

        public DocumentCollectionViewModel GetDocuments()
        {
            IItemsViewModel mainViewModel = (IItemsViewModel)_viewModelManager.Activate("Atom.Client.Desktop.ViewModels.MainViewModel");
            IItemsViewModel documentsViewModel = (IItemsViewModel)mainViewModel.ActivateItem("Atom.Client.Desktop.ViewModels.DocumentCollectionViewModel");
            return (DocumentCollectionViewModel)documentsViewModel;
        }

        public void OpenDocument(FileViewModel file)
        {
            IDocument document = file.Document;
            IObjectDesigner designer = _designerSerializer.Read(document);
            if (designer != null)
            {
                IItemsViewModel mainViewModel = (IItemsViewModel)_viewModelManager.Activate("Atom.Client.Desktop.ViewModels.MainViewModel");
                IItemsViewModel documentsViewModel = (IItemsViewModel)mainViewModel.ActivateItem("Atom.Client.Desktop.ViewModels.DocumentCollectionViewModel");
                documentsViewModel.ActivateItem("Atom.Client.ViewModels.DesignerViewModel", designer);
            }
        }
    }
}
