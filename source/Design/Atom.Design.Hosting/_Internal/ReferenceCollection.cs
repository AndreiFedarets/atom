using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Hosting
{
    internal sealed class ReferenceCollection : LazyReadOnlyDictionary<string, IReference>, IReferenceCollection
    {
        private readonly Project _project;

        public ReferenceCollection(Project project)
        {
            _project = project;
        }

        public event EventHandler ReferenceAdded;

        public event EventHandler ReferenceRemoved;

        IEnumerator<IReference> IEnumerable<IReference>.GetEnumerator()
        {
            lock (Sync)
            {
                return Dictionary.Values.GetEnumerator();
            }
        }

        internal void OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind kind, Microsoft.CodeAnalysis.Project newProject)
        {
            switch (kind)
            {
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectChanged:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectReloaded:
                    lock (Sync)
                    {
                        if (Initialized)
                        {
                            IDictionary<string, IReference> newReferences = GetReferences(newProject);
                            IDictionary<string, IReference> oldReferences = Dictionary;
                            if (newReferences.Count != oldReferences.Count)
                            {
                                HashSet<string> newIds = new HashSet<string>(newReferences.Keys);
                                HashSet<string> oldIds = new HashSet<string>(oldReferences.Keys);
                                if (newIds.Count < oldIds.Count)
                                {
                                    HashSet<string> removedIds = new HashSet<string>(oldIds);
                                    removedIds.ExceptWith(newIds);
                                    foreach (string removedId in removedIds)
                                    {
                                        IReference removedReference = oldReferences[removedId];
                                        Dictionary.Remove(removedId);
                                        ReferenceRemoved?.Invoke(removedReference, EventArgs.Empty);
                                    }
                                }
                                if (newIds.Count > oldIds.Count)
                                {
                                    HashSet<string> addedIds = new HashSet<string>(newIds);
                                    addedIds.ExceptWith(oldIds);
                                    foreach (string addedId in addedIds)
                                    {
                                        IReference addedReference = newReferences[addedId];
                                        Dictionary.Add(addedReference.AssemblyFile, addedReference);
                                        ReferenceAdded?.Invoke(addedReference, EventArgs.Empty);
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        protected override IDictionary<string, IReference> InitializeDictionary()
        {
            return GetReferences(_project.NativeProject);
        }

        private static IDictionary<string, IReference> GetReferences(Microsoft.CodeAnalysis.Project nativeProject)
        {
            Dictionary<string, IReference> dictionary = new Dictionary<string, IReference>();
            Microsoft.CodeAnalysis.Compilation compilation = nativeProject.GetCompilationAsync().Result;
            IEnumerable<Microsoft.CodeAnalysis.MetadataReference> references = compilation.References;

            foreach (Microsoft.CodeAnalysis.MetadataReference metadataReference in references)
            {
                IReference reference;
                if (metadataReference is Microsoft.CodeAnalysis.CompilationReference)
                {
                    reference = new CompilationReference((Microsoft.CodeAnalysis.CompilationReference)metadataReference);
                }
                else
                {
                    reference = new BinaryReference(metadataReference);
                }
                dictionary.Add(reference.AssemblyFile, reference);
            }
            return dictionary;
        }
    }
}
