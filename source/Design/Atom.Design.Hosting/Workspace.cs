using System;

namespace Atom.Design.Hosting
{
    public abstract class Workspace : IWorkspace
    {
        private Solution _solution;

        public Workspace(Microsoft.CodeAnalysis.Workspace nativeWorkspace)
        {
            //TODO: Microsoft.CodeAnalysis.SymbolFinder
            NativeWorkspace = nativeWorkspace;
            _solution = new Solution(NativeWorkspace.CurrentSolution, this);
            NativeWorkspace.WorkspaceFailed += OnWorkspaceFailed;
            NativeWorkspace.WorkspaceChanged += OnWorkspaceChanged;
        }

        public ISolution Solution 
        { 
            get { return _solution; }
        }

        internal Microsoft.CodeAnalysis.Workspace NativeWorkspace { get; private set; }

        public event EventHandler SolutionOpened;

        public event EventHandler SolutionClosed;

        public abstract void OpenSolution(string fileFullName);

        public abstract void CloseSolution();

        public abstract void Attach(int processId);

        protected void RaiseSolutionOpened()
        {
            _solution.Reload();
            SolutionOpened?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseSolutionClosed()
        {
            SolutionClosed?.Invoke(this, EventArgs.Empty);
        }

        private void OnWorkspaceFailed(object sender, Microsoft.CodeAnalysis.WorkspaceDiagnosticEventArgs e)
        {
            Microsoft.CodeAnalysis.WorkspaceDiagnostic diagnostic = e.Diagnostic;
            System.Diagnostics.Debug.WriteLine($"{diagnostic.Kind} : {diagnostic.Message}");
            //MessageBoxIcon icon = diagnostic.Kind == Microsoft.CodeAnalysis.WorkspaceDiagnosticKind.Failure ? MessageBoxIcon.Error : MessageBoxIcon.Warning;
            //MessageBox.Show(diagnostic.Message, "Workspace", MessageBoxButtons.OK, icon);
        }

        private void OnWorkspaceChanged(object sender, Microsoft.CodeAnalysis.WorkspaceChangeEventArgs e)
        {
            _solution.OnWorkspaceChanged(e.Kind, e.NewSolution, e.ProjectId, e.DocumentId);
        }
    }
}
