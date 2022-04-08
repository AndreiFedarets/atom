using Atom.Design.Hosting;
using Microsoft.VisualStudio.Shell;

namespace Atom.Client.VisualStudio
{
    internal sealed class VisualStudioWorkspace : Workspace
    {
        private EnvDTE.DTE _dte;
        private EnvDTE.SolutionEvents _solutionEvents;

        public VisualStudioWorkspace(Microsoft.VisualStudio.LanguageServices.VisualStudioWorkspace workspace, EnvDTE.DTE dte)
            : base(workspace)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _dte = dte;
            EnvDTE.Events events = (EnvDTE80.Events2)_dte.Events;
            _solutionEvents = events.SolutionEvents;
            _solutionEvents.Opened += RaiseSolutionOpened;
            _solutionEvents.AfterClosing += RaiseSolutionClosed;
        }

        public override void Attach(int processId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            EnvDTE.Processes processes = _dte.Debugger.LocalProcesses;
            for (int i = 1; i <= processes.Count; i++)
            {
                EnvDTE.Process process = processes.Item(i);
                if (process.ProcessID == processId)
                {
                    process.Attach();
                    Releaser.Release(ref process);
                    break;
                }
                Releaser.Release(ref process);
            }
            Releaser.Release(ref processes);
        }

        public override void CloseSolution()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            EnvDTE.Solution solution = _dte.Solution;
            if (solution != null)
            {
                solution.Close();
            }
        }

        public override void OpenSolution(string fileFullName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _dte.Solution.Open(fileFullName);
        }
    }
}
