using Caliburn.Micro;

namespace Atom.Design.Hosting
{
    internal sealed class MSBuildWorkspace : Workspace
    {
        private readonly Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace _workspace;

        public MSBuildWorkspace(Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace workspace)
            : base(workspace)
        {
            _workspace = workspace;
        }

        public override void OpenSolution(string fileFullName)
        {
            _workspace.OpenSolutionAsync(fileFullName).ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    Execute.OnUIThread(RaiseSolutionOpened);
                }
            });
        }

        public override void CloseSolution()
        {
            _workspace.CloseSolution();
            RaiseSolutionClosed();
        }

        public override void Attach(int processId)
        {
            throw new System.NotImplementedException();
        }
    }
}
