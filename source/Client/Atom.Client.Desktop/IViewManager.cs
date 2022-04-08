using Atom.Client.Desktop.ViewModels;
using Atom.Client.Desktop.ViewModels.SolutionTree;

namespace Atom.Client.Desktop
{
    public interface IViewManager
    {
        string BrowseSolution();

        void OpenDocument(FileViewModel file);

        DocumentCollectionViewModel GetDocuments();
    }
}
