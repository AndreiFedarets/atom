using System.Collections.Generic;
using System.Windows.Input;
using Atom.Client.Win.SolutionExplorer.Commands;
using Atom.Design;

namespace Atom.Client.Win.SolutionExplorer
{
    public sealed class ProjectItem : BaseItem
    {
        private readonly IWindowsManager _windowsManager;
        private readonly IProject _project;

        public ProjectItem(IProject project, IWindowsManager windowsManager, BaseItem parent)
            : base(project, parent)
        {
            _project = project;
            _windowsManager = windowsManager;
        }

        public override string DisplayName
        {
            get { return _project.Name; }
        }

        protected override void FillChildren()
        {
            base.FillChildren();
            AddItem(new ReferenceFolderItem(_project, _windowsManager, this));
        }

        protected override void FillCommands(List<ICommand> commands)
        {
            base.FillCommands(commands);
            commands.Add(new RenameProjectCommand(_project, _windowsManager));
        }
    }
}
