using System.Collections.Generic;
using Atom.Client.Win.SolutionExplorer;
using Atom.Design;

namespace Atom.Client.Win.ViewModels
{
    public sealed class SolutionExplorerViewModel : ViewModel
    {
        public SolutionExplorerViewModel(ISolution solution, IWindowsManager windowsManager)
        {
            SolutionItems = new[] { new SolutionItem(solution, windowsManager, null) };
        }

        public override string DisplayName
        {
            get { return "Solution Explorer"; }
            set { }
        }

        public IEnumerable<BaseItem> SolutionItems  { get; private set; }
    }
}
