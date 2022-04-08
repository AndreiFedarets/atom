using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atom.Client.Desktop.ViewModels.SolutionTree
{
    internal sealed class SolutionItemFilter : ISolutionItemFilter
    {
        private readonly List<string> _ignoredDirectories;

        public SolutionItemFilter()
        {
            _ignoredDirectories = new List<string> { "properties" };
        }

        public bool Display(ItemViewModel item)
        {
            if (item is SolutionViewModel)
            {
                return true;
            }
            if (item is ProjectViewModel)
            {
                return true;
            }
            if (item is DirectoryViewModel)
            {
                return Display((DirectoryViewModel)item);
            }
            if (item is FileViewModel)
            {
                return Display((FileViewModel)item);
            }
            return false;
        }

        private bool Display(DirectoryViewModel item)
        {
            if (string.Equals(item.Name, "properties", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        private bool Display(FileViewModel item)
        {
            string extension = Path.GetExtension(item.Name);
            return string.Equals(extension, Design.Constants.ActionDesignerDocumentExtension, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, Design.Constants.WorkflowDesignerDocumentExtension, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, Design.Constants.TableDesignerDocumentExtension, StringComparison.OrdinalIgnoreCase);
        }
    }
}
