using Atom.Design.Hosting;

namespace Atom.Client.Desktop.ViewModels.SolutionTree
{
    public sealed class FileViewModel : ItemViewModel
    {
        public FileViewModel(IDocument document, ItemViewModel parent)
            : base(parent)
        {
            Document = document;
        }

        public IDocument Document { get; private set; }

        public override string Name
        {
            get { return Document.Name; }
        }
    }
}
