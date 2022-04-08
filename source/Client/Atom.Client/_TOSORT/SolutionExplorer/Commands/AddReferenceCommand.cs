using System.Collections.Generic;
using Atom.Design;
using Atom.Runtime;

namespace Atom.Client.Win.SolutionExplorer.Commands
{
    public class AddReferenceCommand : ItemCommand
    {
        private readonly IWindowsManager _windowsManager;
        private readonly IProject _project;

        public AddReferenceCommand(IProject project, IWindowsManager windowsManager)
        {
            _project = project;
            _windowsManager = windowsManager;
        }

        public override string Header
        {
            get { return "Add Reference"; }
        }

        public override void Execute()
        {
            IEnumerable<IAssembly> assemblies = _windowsManager.ShowSelectAssembly(_project.Solution, _project);
            foreach (IAssembly assembly in assemblies)
            {
                _project.References.Add(assembly);
            }
        }
    }
}
