using Atom.Design.Hosting;
using Atom.Design.Reflection.Code.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design.Reflection.Code
{
    public abstract class ObjectCollection<TReference, TObject> : ReadOnlyDictionary<TReference, TObject>
    {
        private readonly Dictionary<Guid, IList<TObject>> _sources;
        private readonly IProject _project;
        private readonly ICodeParser _codeParser;

        public ObjectCollection(IProject project, ICodeParser codeParser)
            : base(new Dictionary<TReference, TObject>())
        {
            _project = project;
            _project.Documents.DocumentAdded += OnDocumentAdded;
            _project.Documents.DocumentRemoved += OnDocumentRemoved;
            _codeParser = codeParser;
            _sources = new Dictionary<Guid, IList<TObject>>();
            InitializeCollection();
        }

        protected abstract TReference GetReference(TObject @object);

        private void InitializeCollection()
        {
            foreach (IDocument document in _project.Documents)
            {
                AddDocument(document);
            }
        }

        private void AddDocument(IDocument document)
        {
            IList<TObject> objects = _codeParser.Parse<TObject>(document);
            foreach (TObject @object in objects)
            {
                TReference reference = GetReference(@object);
                Dictionary.Add(reference, @object);
            }
            _sources[document.Id] = objects;
            document.DocumentChanged += OnDocumentChanged;
        }

        private void RemoveDocument(IDocument document)
        {
            document.DocumentChanged -= OnDocumentChanged;
            IList<TObject> objects = _sources[document.Id];
            foreach (TObject @object in objects)
            {
                TReference reference = GetReference(@object);
                Dictionary.Remove(reference);
            }
            _sources.Remove(document.Id);
        }

        private void OnDocumentAdded(object sender, EventArgs e)
        {
            IDocument document = (IDocument)sender;
            AddDocument(document);
        }

        private void OnDocumentChanged(object sender, EventArgs e)
        {
            IDocument document = (IDocument)sender;
            RemoveDocument(document);
            AddDocument(document);
        }

        private void OnDocumentRemoved(object sender, EventArgs e)
        {
            IDocument document = (IDocument)sender;
            RemoveDocument(document);
        }
    }
}
