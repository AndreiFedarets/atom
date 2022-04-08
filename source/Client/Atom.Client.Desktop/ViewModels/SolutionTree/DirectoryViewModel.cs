using Atom.Design.Hosting;
using System;
using System.Collections.Generic;

namespace Atom.Client.Desktop.ViewModels.SolutionTree
{
    public sealed class DirectoryViewModel : ContainerItemViewModel
    {
        private readonly IProject _project;
        private readonly string _name;

        public DirectoryViewModel(string name, IProject project, ItemViewModel parent, ISolutionItemFilter filter)
            : base(parent, filter)
        {
            _name = name;
            _project = project;
        }

        public override string Name
        {
            get { return _name; }
        }

        protected override IEnumerable<ItemViewModel> GetItems()
        {
            List<string> folders = new List<string>();
            DirectoryViewModel directory = this;
            while (directory != null)
            {
                folders.Insert(0, directory.Name);
                directory = directory.Parent as DirectoryViewModel;
            }
            return GetChildren(_project, folders, this, ItemFilter);
        }

        internal static List<ItemViewModel> GetChildren(IProject project, IList<string> folders, ItemViewModel parentViewModel, ISolutionItemFilter itemFilter)
        {
            List<ItemViewModel> collection = new List<ItemViewModel>();
            List<IDocument> descendantDocuments = new List<IDocument>();
            List<IDocument> childrenDocuments = new List<IDocument>();
            const string pathSeparator = "\\";
            string path = string.Join(pathSeparator, folders);

            foreach (IDocument document in project.Documents)
            {
                string documentPath = string.Join(pathSeparator, document.Folders);
                if (string.Equals(documentPath, path, StringComparison.OrdinalIgnoreCase))
                {
                    childrenDocuments.Add(document);
                }
                else if (documentPath.StartsWith(path, StringComparison.OrdinalIgnoreCase))
                {
                    descendantDocuments.Add(document);
                }
            }

            descendantDocuments.Sort((x, y) => string.Compare(x.Name, y.Name));
            childrenDocuments.Sort((x, y) => string.Compare(x.Name, y.Name));

            HashSet<string> processedFolders = new HashSet<string>();
            int currentPathLevel = folders.Count;
            foreach (IDocument document in descendantDocuments)
            {
                IReadOnlyList<string> documentFolders = document.Folders;
                string folderName = documentFolders[currentPathLevel];
                string folderKey = folderName.ToLowerInvariant();
                if (!processedFolders.Contains(folderKey))
                {
                    processedFolders.Add(folderKey);
                    DirectoryViewModel directoryViewModel = new DirectoryViewModel(folderName, project, parentViewModel, itemFilter);
                    collection.Add(directoryViewModel);
                }
            }

            foreach (IDocument document in childrenDocuments)
            {
                FileViewModel fileViewModel = new FileViewModel(document, parentViewModel);
                collection.Add(fileViewModel);
            }
            return collection;
        }

        public static void SplitDocuments(string path, IEnumerable<IDocument> source, List<IDocument> children, List<IDocument> descendants)
        {
        }
    }
}
