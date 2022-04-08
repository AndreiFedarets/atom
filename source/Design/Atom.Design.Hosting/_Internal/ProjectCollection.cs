using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Hosting
{
    internal sealed class ProjectCollection : LazyReadOnlyDictionary<Guid, Project>, IProjectCollection
    {
        private readonly Solution _solution;

        public ProjectCollection(Solution solution)
        {
            _solution = solution;
        }

        public event EventHandler ProjectAdded;

        public event EventHandler ProjectRemoved;

        IEnumerator<IProject> IEnumerable<IProject>.GetEnumerator()
        {
            lock (Sync)
            {
                return Dictionary.Values.GetEnumerator();
            }
        }

        internal void OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind kind, Microsoft.CodeAnalysis.Solution newSolution, Microsoft.CodeAnalysis.Solution oldSolution, Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.DocumentId documentId)
        {
            Project project = null;
            switch (kind)
            {
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.SolutionReloaded:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.SolutionAdded:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.SolutionChanged:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.SolutionCleared:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.SolutionRemoved:
                    if (Initialized)
                    {
                        if (!oldSolution.ProjectIds.SequenceEqual(newSolution.ProjectIds))
                        {
                            HashSet<Microsoft.CodeAnalysis.ProjectId> newIds = new HashSet<Microsoft.CodeAnalysis.ProjectId>(newSolution.ProjectIds);
                            HashSet<Microsoft.CodeAnalysis.ProjectId> oldIds = new HashSet<Microsoft.CodeAnalysis.ProjectId>(oldSolution.ProjectIds);
                            if (newIds.Count < oldIds.Count)
                            {
                                HashSet<Microsoft.CodeAnalysis.ProjectId> removedIds = new HashSet<Microsoft.CodeAnalysis.ProjectId>(oldIds);
                                removedIds.ExceptWith(newIds);
                                foreach (Microsoft.CodeAnalysis.ProjectId removedId in removedIds)
                                {
                                    OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectRemoved, newSolution, oldSolution, removedId, null);
                                }
                            }
                            if (newIds.Count > oldIds.Count)
                            {
                                HashSet<Microsoft.CodeAnalysis.ProjectId> addedIds = new HashSet<Microsoft.CodeAnalysis.ProjectId>(newIds);
                                addedIds.ExceptWith(oldIds);
                                foreach (Microsoft.CodeAnalysis.ProjectId addedId in addedIds)
                                {
                                    OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectAdded, newSolution, oldSolution, addedId, null);
                                }
                            }
                        }
                        foreach (Microsoft.CodeAnalysis.ProjectId newId in newSolution.ProjectIds)
                        {
                            if (oldSolution.GetProject(newId) != null)
                            {
                                OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectChanged, newSolution, oldSolution, newId, null);
                            }
                        }
                    }
                    break;
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectAdded:
                    lock (Sync)
                    {
                        if (Initialized)
                        {
                            if (!Dictionary.ContainsKey(projectId.Id))
                            {
                                Microsoft.CodeAnalysis.Project nativeProject = newSolution.GetProject(projectId);
                                project = new Project(nativeProject, _solution);
                                Dictionary.Add(project.Id, project);
                            }
                            else
                            {
                                System.Diagnostics.Debug.Fail($"Project with id {projectId} already exists");
                            }
                        }
                    }
                    if (project != null)
                    {
                        ProjectAdded?.Invoke(project, EventArgs.Empty);
                    }
                    break;
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectRemoved:
                    lock (Sync)
                    {
                        if (Initialized)
                        {
                            if (Dictionary.TryGetValue(projectId.Id, out project))
                            {
                                Dictionary.Remove(projectId.Id);
                            }
                            else
                            {
                                System.Diagnostics.Debug.Fail($"There is no project with id {projectId}");
                            }
                        }
                    }
                    if (project != null)
                    {
                        ProjectRemoved?.Invoke(project, EventArgs.Empty);
                    }
                    break;
                default:
                    lock (Sync)
                    {
                        if (Initialized)
                        {
                            if (!Dictionary.TryGetValue(projectId.Id, out project))
                            {
                                System.Diagnostics.Debug.Fail($"There is no project with id {projectId}");
                            }
                        }
                    }
                    if (project != null)
                    {
                        Microsoft.CodeAnalysis.Project nativeProject = newSolution.GetProject(projectId);
                        project.OnWorkspaceChanged(kind, nativeProject, documentId);
                    }
                    break;
            }
        }

        protected override IDictionary<Guid, Project> InitializeDictionary()
        {
            Dictionary<Guid, Project> dictionary = new Dictionary<Guid, Project>();
            Microsoft.CodeAnalysis.Solution nativeSolution = _solution.NativeSolution;
            foreach (Microsoft.CodeAnalysis.Project nativeProject in nativeSolution.Projects)
            {
                Project project = new Project(nativeProject, _solution);
                dictionary.Add(project.Id, project);
            }
            return dictionary;
        }
    }
}
