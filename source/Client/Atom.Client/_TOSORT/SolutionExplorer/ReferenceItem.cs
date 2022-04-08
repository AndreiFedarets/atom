using Atom.Client.Win.SolutionExplorer.Commands;
using System.Collections.Generic;
using System.Windows.Input;
using Atom.Design;

namespace Atom.Client.Win.SolutionExplorer
{
    public sealed class ReferenceItem : BaseItem
    {
        private readonly IWindowsManager _windowsManager;
        private readonly IAssemblyReferenceCollection _references;
        private readonly IAssemblyReference _reference;

        public ReferenceItem(IAssemblyReference reference, IAssemblyReferenceCollection references, IWindowsManager windowsManager, BaseItem parent)
            : base(reference, parent)
        {
            _reference = reference;
            _references = references;
            _windowsManager = windowsManager;
        }

        public override string DisplayName
        {
            get { return string.Format("{0}, Version={1}, Type={2}", _reference.Metadata.Name, _reference.Metadata.Version, _reference.Metadata.Type); }
        }

        protected override void FillCommands(List<ICommand> commands)
        {
            base.FillCommands(commands);
            commands.Add(new RemoveReferenceCommand(_reference, _references, _windowsManager));
        }
    }
}
