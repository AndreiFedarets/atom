using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;
using Layex.ViewModels;
using System;
using System.IO;
using System.Windows;

namespace Atom.Client.ViewModels
{
    public class DesignerViewModel : ViewModel, INotifyDocumentChanged
    {
        private readonly IViewModelManager _viewModelManager;
        private bool _hasChanges;

        public DesignerViewModel(IObjectDesigner designer, IViewModelManager viewModelManager)
        {
            Designer = designer;
            _viewModelManager = viewModelManager;
        }

        public override string DisplayName 
        {
            get
            {
                string displayName = Designer.Document.Name;
                if (_hasChanges)
                {
                    displayName += " *";
                }
                return displayName;
            }
            set { }
        }

        public IObjectDesigner Designer { get; private set; }

        public event EventHandler DocumentChanged;
        
        public void Save()
        {
            Services.Serializer.Write(Designer);
            if (Services.Validator.Validate(Designer))
            {
                IDocument document = Designer.Document;
                IProject project = document.Project;
                ISolution solution = project.Solution;
                string codeFileExtension = DocumentExtension.GetCodeExtension(project.Language);
                string codeFileFullName = document.FullName + codeFileExtension;
                IDocument codeDocument = solution.FindDocument(codeFileFullName);
                if (codeDocument == null)
                {
                    //Create document
                }
                string codeFileContent = Services.CodeGenerator.Generate(Designer);
                using (FileStream stream = new FileStream(codeFileFullName, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(codeFileContent);
                    }
                }
            }
            _hasChanges = false;
            NotifyOfPropertyChange(() => DisplayName);
        }

        private void OnDesignerChanged(object sender, EventArgs eventArgs)
        {
            _hasChanges = true;
            DocumentChanged?.Invoke(this, EventArgs.Empty);
            NotifyOfPropertyChange(() => DisplayName);
        }
    }
}
