using System;
using System.Collections.Generic;
using System.Windows.Input;
using Atom.Client.Win.SolutionExplorer.Commands;
using Atom.Design;

namespace Atom.Client.Win.SolutionExplorer
{
    public sealed class SolutionItem : BaseItem
    {
        private readonly IWindowsManager _windowsManager;
        private readonly ISolution _solution;

        public SolutionItem(ISolution solution, IWindowsManager windowsManager, BaseItem parent)
            : base(solution, parent)
        {
            _solution = solution;
            _solution.Projects.ProjectAdded += OnProjectAdded;
            _solution.Projects.ProjectRemoved += OnProjectRemoved;
            _windowsManager = windowsManager;
            IsExpanded = true;
        }

        public override string DisplayName
        {
            get { return _solution.Name; }
        }

        protected override void FillChildren()
        {
            base.FillChildren();
            foreach (IProject project in _solution.Projects)
            {
                ProjectItem item = new ProjectItem(project, _windowsManager, this);
                AddItem(item);
            }
        }

        protected override void FillCommands(List<ICommand> commands)
        {
            base.FillCommands(commands);
            commands.Add(new CreateProjectCommand(_solution, _windowsManager));
            commands.Add(new RenameSolutionCommand(_solution, _windowsManager));
        }

        private void OnProjectAdded(object sender, ProjectEventArgs eventArgs)
        {
            ProjectItem item = new ProjectItem(eventArgs.Project, _windowsManager, this);
            AddItem(item);
        }

        private void OnProjectRemoved(object sender, ProjectEventArgs eventArgs)
        {
            ProjectItem item = (ProjectItem)Find(x => x.UnderlyingObject == eventArgs.Project);
            AddItem(item);
        }
    }
}
