using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Hosting
{
    internal sealed class DocumentCollection : LazyReadOnlyDictionary<Guid, Document>, IDocumentCollection
    {
        private readonly Project _project;

        public DocumentCollection(Project project)
        {
            _project = project;
        }

        public event EventHandler DocumentAdded;

        public event EventHandler DocumentRemoved;

        IEnumerator<IDocument> IEnumerable<IDocument>.GetEnumerator()
        {
            lock (Sync)
            {
                return Dictionary.Values.GetEnumerator();
            }
        }

        internal void OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind kind, Microsoft.CodeAnalysis.Project newProject, Microsoft.CodeAnalysis.Project oldProject, Microsoft.CodeAnalysis.DocumentId documentId)
        {
            Document document = null;
            switch (kind)
            {
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectReloaded:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectChanged:
                    if (Initialized)
                    {
                        CalculateAddedRemovedDocuments(newProject, oldProject, Microsoft.CodeAnalysis.WorkspaceChangeKind.DocumentAdded, 
                            Microsoft.CodeAnalysis.WorkspaceChangeKind.DocumentRemoved, p => p.DocumentIds);

                        CalculateAddedRemovedDocuments(newProject, oldProject, Microsoft.CodeAnalysis.WorkspaceChangeKind.AdditionalDocumentAdded,
                            Microsoft.CodeAnalysis.WorkspaceChangeKind.AdditionalDocumentRemoved, p => p.AdditionalDocumentIds);
                    }
                    break;
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.AdditionalDocumentAdded:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.DocumentAdded:
                    lock (Sync)
                    {
                        if (Initialized)
                        {
                            if (!Dictionary.ContainsKey(documentId.Id))
                            {
                                Microsoft.CodeAnalysis.TextDocument nativeDocument = newProject.GetDocument(documentId) ?? newProject.GetAdditionalDocument(documentId);
                                document = new Document(nativeDocument, _project);
                                Dictionary.Add(document.Id, document);
                            }
                            else
                            {
                                System.Diagnostics.Debug.Fail($"Document with id {documentId} already exists");
                            }
                        }
                    }
                    if (document != null)
                    {
                        DocumentAdded?.Invoke(document, EventArgs.Empty);
                    }
                    break;
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.AdditionalDocumentRemoved:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.DocumentRemoved:
                    lock (Sync)
                    {
                        if (Initialized)
                        {
                            if (Dictionary.TryGetValue(documentId.Id, out document))
                            {
                                Dictionary.Remove(documentId.Id);
                            }
                            else
                            {
                                System.Diagnostics.Debug.Fail($"There is no document with id {documentId}");
                            }
                        }
                    }
                    if (document != null)
                    {
                        DocumentRemoved?.Invoke(document, EventArgs.Empty);
                    }
                    break;
                default:
                    lock (Sync)
                    {
                        if (Initialized)
                        {
                            if (!Dictionary.TryGetValue(documentId.Id, out document))
                            {
                                System.Diagnostics.Debug.Fail($"There is no document with id {documentId}");
                            }
                        }
                    }
                    if (document != null)
                    {
                        Microsoft.CodeAnalysis.TextDocument nativeDocument = newProject.GetDocument(documentId) ?? newProject.GetAdditionalDocument(documentId);
                        document.OnWorkspaceChanged(kind, nativeDocument);
                    }
                    break;
            }
        }

        protected override IDictionary<Guid, Document> InitializeDictionary()
        {
            Dictionary<Guid, Document> dictionary = new Dictionary<Guid, Document>();
            Microsoft.CodeAnalysis.Project nativeProject = _project.NativeProject;
            foreach (Microsoft.CodeAnalysis.TextDocument nativeDocument in nativeProject.Documents.Concat(nativeProject.AdditionalDocuments))
            {
                Document document = new Document(nativeDocument, _project);
                dictionary.Add(document.Id, document);
            }
            return dictionary;
        }

        private void CalculateAddedRemovedDocuments(Microsoft.CodeAnalysis.Project newProject, Microsoft.CodeAnalysis.Project oldProject,
            Microsoft.CodeAnalysis.WorkspaceChangeKind addedKind, Microsoft.CodeAnalysis.WorkspaceChangeKind removedKind,
            Func<Microsoft.CodeAnalysis.Project, IReadOnlyList<Microsoft.CodeAnalysis.DocumentId>> getDocumentIds)
        {
            if (!getDocumentIds(newProject).SequenceEqual(getDocumentIds(oldProject)))
            {
                HashSet<Microsoft.CodeAnalysis.DocumentId> newIds = new HashSet<Microsoft.CodeAnalysis.DocumentId>(getDocumentIds(newProject));
                HashSet<Microsoft.CodeAnalysis.DocumentId> oldIds = new HashSet<Microsoft.CodeAnalysis.DocumentId>(getDocumentIds(oldProject));

                if (newIds.Count < oldIds.Count)
                {
                    HashSet<Microsoft.CodeAnalysis.DocumentId> removedIds = new HashSet<Microsoft.CodeAnalysis.DocumentId>(oldIds);
                    removedIds.ExceptWith(newIds);
                    foreach (Microsoft.CodeAnalysis.DocumentId removedId in removedIds)
                    {
                        OnWorkspaceChanged(removedKind, newProject, oldProject, removedId);
                    }
                }

                if (newIds.Count > oldIds.Count)
                {
                    HashSet<Microsoft.CodeAnalysis.DocumentId> addedIds = new HashSet<Microsoft.CodeAnalysis.DocumentId>(newIds);
                    addedIds.ExceptWith(oldIds);
                    foreach (Microsoft.CodeAnalysis.DocumentId addedId in addedIds)
                    {
                        OnWorkspaceChanged(addedKind, newProject, oldProject, addedId);
                    }
                }
            }
        }
    }
}
