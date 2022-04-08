using System.Collections.Generic;
using System.Windows.Input;
using Atom.Client.Win.SolutionExplorer.Commands;
using Atom.Design;

namespace Atom.Client.Win.SolutionExplorer
{
    public sealed class ReferenceFolderItem : BaseItem
    {
        private readonly IWindowsManager _windowsManager;
        private readonly IProject _project;

        public ReferenceFolderItem(IProject project, IWindowsManager windowsManager, BaseItem parent)
            : base(project.References, parent)
        {
            _project = project;
            _windowsManager = windowsManager;
            _project.References.ReferencesAdded += OnReferencesAdded;
            _project.References.ReferencesRemoved += OnReferencesRemoved;
        }

        public override string DisplayName
        {
            get { return "References"; }
        }

        protected override void FillCommands(List<ICommand> commands)
        {
            base.FillCommands(commands);
            commands.Add(new AddReferenceCommand(_project, _windowsManager));
        }

        private void OnReferencesAdded(object sender, AssemblyReferenceEventArgs e)
        {
            ReferenceItem item = new ReferenceItem(e.Reference, _project.References, _windowsManager, this);
            AddItem(item);
        }

        private void OnReferencesRemoved(object sender, AssemblyReferenceEventArgs e)
        {
            ReferenceItem item = (ReferenceItem)Find(x => x.UnderlyingObject == e.Reference);
            RemoveItem(item);
        }

        protected override void FillChildren()
        {
            foreach (IAssemblyReference reference in _project.References)
            {
                ReferenceItem item = new ReferenceItem(reference, _project.References, _windowsManager, this);
                AddItem(item);
            }
        }
    }
}
