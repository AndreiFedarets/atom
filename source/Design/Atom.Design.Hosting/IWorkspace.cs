using System;

namespace Atom.Design.Hosting
{
    public interface IWorkspace
    {
        ISolution Solution { get; }

        event EventHandler SolutionOpened;

        event EventHandler SolutionClosed;

        void OpenSolution(string fileFullName);

        void CloseSolution();

        void Attach(int processId);
    }
}
