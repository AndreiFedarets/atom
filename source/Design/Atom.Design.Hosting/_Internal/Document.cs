using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Hosting
{
    internal sealed class Document : IDocument
    {
        public Document(Microsoft.CodeAnalysis.TextDocument nativeDocument, Project project)
        {
            NativeDocument = nativeDocument;
            Project = project;
        }

        public Guid Id 
        { 
            get { return NativeDocument.Id.Id; }
        }

        public string Namespace
        {
            get
            {
                List<string> parts = Folders.ToList();
                parts.Insert(0, NativeDocument.Project.Name);
                string namespaceName = string.Join(".", parts);
                //string projectPath = System.IO.Path.GetDirectoryName(Project.Path);
                //string namespaceName = Path.Substring(projectPath.Length).Trim('\\');
                //namespaceName = namespaceName.Replace("\\", ".");
                return namespaceName;
            }
        }

        public IReadOnlyList<string> Folders
        {
            get { return NativeDocument.Folders; }
        }

        public string Name
        {
            get { return NativeDocument.Name; }
        }

        public string Path
        {
            get { return System.IO.Path.GetDirectoryName(NativeDocument.FilePath); }
        }

        public string FullName
        {
            get { return NativeDocument.FilePath; }
        }

        public DocumentType DocumentType
        {
            get { return DocumentExtension.GetDocumentType(this); }
        }

        public IProject Project { get; private set; }

        public IDocument Designer
        {
            get
            {
                IDocument document = null;
                string fileName = Name;
                if (DocumentExtension.GetDocumentType(this) == DocumentType.Code)
                {
                    string designerFileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    string designerFileFullName = System.IO.Path.Combine(Path, designerFileName);
                    document = Project.Solution.FindDocument(designerFileFullName);
                }
                return document;
            }
        }

        internal Microsoft.CodeAnalysis.TextDocument NativeDocument { get; private set; }

        public event EventHandler DocumentChanged;

        public string GetSourceText()
        {
            SourceText sourceText = NativeDocument.GetTextAsync().Result;
            return sourceText.ToString();
        }

        public object GetSyntaxRoot()
        {
            Microsoft.CodeAnalysis.Document document = NativeDocument as Microsoft.CodeAnalysis.Document;
            Microsoft.CodeAnalysis.SyntaxNode syntaxRoot = document?.GetSyntaxRootAsync().Result;
            return syntaxRoot;
        }

        internal void OnWorkspaceChanged(Microsoft.CodeAnalysis.WorkspaceChangeKind kind, Microsoft.CodeAnalysis.TextDocument newDocument)
        {
            NativeDocument = newDocument;
            switch (kind)
            {
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.DocumentReloaded:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.DocumentChanged:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.AdditionalDocumentReloaded:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.AdditionalDocumentChanged:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.DocumentInfoChanged:
                    DocumentChanged?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
    }
}
