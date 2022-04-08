using System;
using System.Reflection;

namespace Atom.Design.Hosting
{
    internal sealed class Project : IProject
    {
        private readonly DocumentCollection _documents;
        private readonly ReferenceCollection _references;

        public Project(Microsoft.CodeAnalysis.Project nativeProject, Solution solution)
        {
            NativeProject = nativeProject;
            Solution = solution;
            _documents = new DocumentCollection(this);
            _references = new ReferenceCollection(this);
        }

        public Guid Id 
        { 
            get { return NativeProject.Id.Id; }
        }

        public string Name
        {
            get { return NativeProject.Name; }
        }

        public string Path
        {
            get { return System.IO.Path.GetDirectoryName(NativeProject.FilePath); }
        }

        public string FullName
        {
            get { return NativeProject.FilePath; }
        }

        public string OutputFilePath
        {
            get { return NativeProject.OutputFilePath; }
        }
        
        public AssemblyName AssemblyName
        {
            get
            {
                Microsoft.CodeAnalysis.Compilation compilation = GetCompilation();
                Microsoft.CodeAnalysis.AssemblyIdentity identity = compilation.Assembly.Identity;
                return new AssemblyName(identity.ToString());
            }
        }

        public CodeLanguage Language
        {
            get { return DocumentExtension.GetProjectLanguage(NativeProject.Language); }
        }

        public IDocumentCollection Documents
        {  
            get { return _documents; }
        }

        public IReferenceCollection References
        {
            get { return _references; }
        }

        public ISolution Solution { get; private set; }

        public Microsoft.CodeAnalysis.Compilation GetCompilation()
        {
            return NativeProject.GetCompilationAsync().Result;
        }

        internal Microsoft.CodeAnalysis.Project NativeProject { get; private set; }

        internal void OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind kind, Microsoft.CodeAnalysis.Project newProject, Microsoft.CodeAnalysis.DocumentId documentId)
        {
            Microsoft.CodeAnalysis.Project oldProject = NativeProject;
            NativeProject = newProject;
            _documents.OnWorkspaceChanged(kind, newProject, oldProject, documentId);
            _references.OnWorkspaceChanged(kind, newProject);
        }
    }
}
