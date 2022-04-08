using System;
using System.IO;

namespace Atom.Design.Hosting
{
    internal sealed class Solution : ISolution
    {
        private readonly Workspace _workspace;
        private readonly ProjectCollection _projects;

        public Solution(Microsoft.CodeAnalysis.Solution nativeSolution, Workspace workspace)
        {
            NativeSolution = nativeSolution;
            _workspace = workspace;
            _projects = new ProjectCollection(this);
        }

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(NativeSolution.FilePath); }
        }

        public IProjectCollection Projects
        { 
            get { return _projects; }
        }

        internal Microsoft.CodeAnalysis.Solution NativeSolution { get; private set; }

        public IDocument FindDocument(string fileFullName)
        {
            foreach (IProject project in Projects)
            {
                foreach (IDocument document in project.Documents)
                {
                    if (string.Equals(document.FullName, fileFullName, StringComparison.OrdinalIgnoreCase))
                    {
                        return document;
                    }
                }
            }
            return null;
        }

        public void Reload()
        {
            Microsoft.CodeAnalysis.Solution newSolution = _workspace.NativeWorkspace.CurrentSolution;
            OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind.SolutionReloaded, newSolution, null, null);
        }

        internal void OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind kind, Microsoft.CodeAnalysis.Solution newSolution, Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.DocumentId documentId)
        {
            Microsoft.CodeAnalysis.Solution oldSolution = NativeSolution;
            NativeSolution = newSolution;
            _projects.OnWorkspaceChanged(kind, newSolution, oldSolution, projectId, documentId);
        }
    }
}
