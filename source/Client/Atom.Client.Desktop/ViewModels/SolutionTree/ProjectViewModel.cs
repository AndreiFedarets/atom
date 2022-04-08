using Atom.Design.Hosting;
using System.Collections.Generic;

namespace Atom.Client.Desktop.ViewModels.SolutionTree
{
    public sealed class ProjectViewModel : ContainerItemViewModel
    {
        private readonly IProject _project;

        public ProjectViewModel(IProject project, SolutionViewModel parent, ISolutionItemFilter filter)
            : base(parent, filter)
        {
            _project = project;
        }

        public override string Name
        {
            get { return _project.Name; }
        }

        protected override IEnumerable<ItemViewModel> GetItems()
        {
            return DirectoryViewModel.GetChildren(_project, new List<string>(), this, ItemFilter);
        }
    }
}
