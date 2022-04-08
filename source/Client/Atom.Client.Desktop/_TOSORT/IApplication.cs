using System;

namespace Atom.Design
{
    public interface IApplication
    {
        //IRuntime Runtime { get; }

        IAssemblyCollection Assemblies { get; }

        //IDirectories Directories { get; }

        //IActionArgumentBinder ArgumentBinder { get; }

        ISolution CurrentSolution { get; }

        event EventHandler<SolutionEventArgs> SolutionOpened;

        event EventHandler<SolutionEventArgs> SolutionClosed;

        void OpenSolution(string fullName);

        void CreateSolution(string path, string name);

        void SaveSolution();

        void CloseSolution();
    }
}
