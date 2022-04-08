using Atom.Design.Hosting;
using System.Collections.Generic;

namespace Atom.Client.Desktop.ViewModels.SolutionTree
{
    public sealed class SolutionViewModel : ContainerItemViewModel
    {
        private readonly ISolution _solution;

        public SolutionViewModel(ISolution solution, ISolutionItemFilter itemFilter)
            : base(null, itemFilter)
        {
            _solution = solution;
            IsExpanded = true;
        }

        public override string Name
        {
            get { return _solution.Name; }
        }

        protected override IEnumerable<ItemViewModel> GetItems()
        {
            foreach (IProject project in _solution.Projects)
            {
                yield return new ProjectViewModel(project, this, ItemFilter);
            }
        }
    }
}
